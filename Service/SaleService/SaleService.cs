using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs;
using FifoApi.DTOs.SaleDTO;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Extensions;
using FifoApi.Helpers;
using FifoApi.Helpers.SaleHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.SaleInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers.SaleMapper;
using FifoApi.Mappers.StockMapper;
using FifoApi.Models;
using FifoApi.Service.Result;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Service.SaleService
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IStockMovementRepository _stockMovementRepo;
        private readonly ApplicationDBContext _context;
        private readonly ILogger<SaleService> _logger;
        public SaleService(
            ISaleRepository saleRepo,
            IProductRepository productRepo,
            IStockRepository stockRepo,
            IStockMovementRepository stockMovementRepo,
            ApplicationDBContext context,
            ILogger<SaleService> logger
        )
        {
            _saleRepo = saleRepo;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _stockMovementRepo = stockMovementRepo;
            _context = context;
            _logger = logger;
        }
        public async Task<OperationResult<SaleDTO>> CreateSaleAsync(CreateSaleDTO saleDTO)
        {
            return await RetryHelper.RetryOperationWithTransactionAsync(
                _context,
                async () =>
                {
                    var items = saleDTO.Items;
                    var productIds = items.Select(x => x.ProductId).Distinct().ToList();
                    var products = await _productRepo.GetByIdsAsync(productIds);
                    if (products == null || products.Count != productIds.Count)
                    {
                        return OperationResult<SaleDTO>.BadRequest("There are some or all products are not found");
                    }

                    var saleItems = new List<SaleItem>();
                    var adjustQtyList = new List<AdjustStockDTO>();

                    var stocks = await _stockRepo.GetAvailableStockAsync(productIds);

                    if (stocks == null || !stocks.Any())
                    {
                        return OperationResult<SaleDTO>.BadRequest("Stocks not found");
                    }

                    var stockDict = stocks
                        .GroupBy(s => s.ProductId)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    var allocation = await AllocateStocksFIFO(items, stockDict, _stockRepo);

                    if (!allocation.Success)
                    {
                        var outOfStockIds = allocation.OutOfStockIds;
                        HashSet<string> outOfStockProductSKU = new HashSet<string>();
                        foreach (var id in outOfStockIds)
                        {
                            var selectedProduct = products.FirstOrDefault(p => p.Id == id);
                            if (selectedProduct != null)
                            {
                                outOfStockProductSKU.Add(selectedProduct.SKU);
                            }
                        }
                        string message = outOfStockProductSKU.Count > 0 ?
                            "Insufficient stock for products: " + string.Join(", ", outOfStockProductSKU)
                            : allocation.ErrorMessage!;
                        return OperationResult<SaleDTO>.BadRequest(message);
                    }

                    saleItems = allocation.SaleItems;
                    adjustQtyList = allocation.AdjustStocks;

                    var nextSequence = await _saleRepo.GetNextInvoiceSequenceAsync("INV");
                    var invoice = InvoiceNoGenerator.Generate(
                        sequenceNumber: nextSequence
                    );

                    var createdSale = await _saleRepo.CreateSaleAsync(saleDTO.ToSaleFromCreate(invoice, saleItems));
                    if (createdSale == null)
                    {
                        return OperationResult<SaleDTO>.BadRequest("Failed to create sales data");
                    }

                    var isSuccessAdjustStock = await _stockRepo.AdjustListStockQtyAsync(adjustQtyList.ToAdjustListStockQtyDTO(), "-");
                    if (!isSuccessAdjustStock)
                    {
                        return OperationResult<SaleDTO>.BadRequest("Failed to adjust stocks");
                    }

                    var stockMovements = createdSale.ToStockMovements(adjustQtyList);
                    var createdStockMovements = await _stockMovementRepo.CreateStockMovementsAsync(stockMovements);
                    if (createdStockMovements == null)
                    {
                        return OperationResult<SaleDTO>.BadRequest("Failed to create stock movements");
                    }

                    return OperationResult<SaleDTO>.Ok(createdSale.ToSaleDTO());
                },
                maxRetry: 3,
                retryDelayMs: 50,
                logger: _logger,
                transientExceptionFilter: e => e is DbUpdateConcurrencyException
            );
        }

        public async Task<OperationResult<PagedResult<SaleDTO>>> GetAllSaleAsync(SaleQueryObject query)
        {
            try
            {
                var salesQuery = await _saleRepo.GetAllSalesAsync(query);
                if (salesQuery == null || salesQuery.Count() <= 0)
                {
                    return OperationResult<PagedResult<SaleDTO>>.NotFound("Sales not found");
                }
                var pagedResult = await salesQuery.ToPagedResultAsync(
                    query.PageNumber,
                    query.PageSize,
                    s => s.ToSaleDTO()
                );
                return OperationResult<PagedResult<SaleDTO>>.Ok(pagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting sales");
                return OperationResult<PagedResult<SaleDTO>>.InternalServerError();
            }
        }

        public async Task<OperationResult<SaleDetailDTO?>> GetSaleByIdAsync(int id)
        {
            try
            {
                var sale = await _saleRepo.GetByIdAsync(id);
                if (sale == null)
                {
                    return OperationResult<SaleDetailDTO?>.NotFound("Data not found");
                }
                var saleDTO = sale.ToSaleDetailDTO();
                return OperationResult<SaleDetailDTO?>.Ok(saleDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting sale by id");
                return OperationResult<SaleDetailDTO?>.InternalServerError();
            }
        }

        public async Task<OperationResult<SaleDetailDTO?>> GetSaleByInvoiceAsync(string invoice)
        {
            try
            {
                var invNormalize = StringHelper.NormalizeInvoice(invoice);
                var sale = await _saleRepo.GetByInvoiceAsync(invNormalize);
                if (sale == null)
                {
                    return OperationResult<SaleDetailDTO?>.NotFound("Data not found");
                }
                var saleDTO = sale.ToSaleDetailDTO();
                return OperationResult<SaleDetailDTO?>.Ok(saleDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting sale by invoice number");
                return OperationResult<SaleDetailDTO?>.InternalServerError();
            }
        }

        private static async Task<StockAllocationResult> AllocateStocksFIFO(
            List<CreateSaleItemDTO> items,
            Dictionary<int, List<Stock>> stockDict,
            IStockRepository stockRepo
        )
        {
            var outOfStocks = new HashSet<int>();
            var saleItems = new List<SaleItem>();
            var adjustQtyList = new List<AdjustStockDTO>();

            foreach (var item in items)
            {
                if (item.Qty <= 0)
                {
                    return StockAllocationResult.ErrorResult("Qty must be greater than zero", null);
                }

                if (!stockDict.TryGetValue(item.ProductId, out var productStocks))
                {
                    outOfStocks.Add(item.ProductId);
                    continue;
                }

                decimal totalPrice = 0;
                int qtyRemaining = item.Qty;
                var totalGrandStock = await stockRepo.GetGrandTotalStockAsync(item.ProductId);
                if (totalGrandStock < qtyRemaining)
                {
                    outOfStocks.Add(item.ProductId);
                    continue;
                }

                var tempId = Guid.NewGuid();

                foreach (var stock in productStocks)
                {
                    if (qtyRemaining <= 0)
                        break;

                    var deductQty = Math.Min(stock.QtyRemaining, qtyRemaining);

                    totalPrice += deductQty * stock.PurchasePrice;

                    adjustQtyList.Add(stock.ToAdjustStockDTO(deductQty, tempId));

                    qtyRemaining -= deductQty;
                }

                if (qtyRemaining > 0)
                {
                    outOfStocks.Add(item.ProductId);
                    continue;
                }

                saleItems.Add(item.ToSaleItemFromCreate(totalPrice, tempId));
            }

            if (outOfStocks.Count > 0)
            {
                return StockAllocationResult.ErrorResult(
                    "Insufficient stocks",
                    outOfStocks
                );
            }

            return StockAllocationResult.SuccessResult(saleItems, adjustQtyList);
        }
    }
}