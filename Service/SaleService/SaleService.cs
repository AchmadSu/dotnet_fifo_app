using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs;
using FifoApi.DTOs.SaleDTO;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers;
using FifoApi.Helpers.SaleHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.SaleInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers.SaleMapper;
using FifoApi.Mappers.StockMapper;
using FifoApi.Models;
using FifoApi.Service.Result;

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
            using var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                var items = saleDTO.Items;
                var productIds = items.Select(x => x.ProductId).Distinct().ToList();
                var products = await _productRepo.GetByIdsAsync(productIds);
                if (products == null || products.Count != productIds.Count)
                {
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest("There are some or all products are not found");
                }

                var saleItems = new List<SaleItem>();
                var adjustQtyList = new List<AdjustStockDTO>();

                var stocks = await _stockRepo.GetAvailableStockAsync(productIds);

                if (stocks == null || !stocks.Any())
                {
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest("Stocks not found");
                }

                var stockDict = stocks
                    .GroupBy(s => s.ProductId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var allocation = AllocateStocksFIFO(items, stockDict);

                if (!allocation.Success)
                {
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest(allocation.ErrorMessage!);
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
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest("Failed to create sales data");
                }

                var isSuccessAdjustStock = await _stockRepo.AdjustListStockQtyAsync(adjustQtyList.ToAdjustListStockQtyDTO(), "-");
                if (!isSuccessAdjustStock)
                {
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest("Failed to adjust stocks");
                }

                var stockMovements = createdSale.ToStockMovements(adjustQtyList);
                var createdStockMovements = await _stockMovementRepo.CreateStockMovementsAsync(stockMovements);
                if (createdStockMovements == null)
                {
                    await trx.RollbackAsync();
                    return OperationResult<SaleDTO>.BadRequest("Failed to create stock movements");
                }

                await trx.CommitAsync();
                return OperationResult<SaleDTO>.Ok(createdSale.ToSaleDTO());
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                _logger.LogError(e, "Error while creating sale");
                return OperationResult<SaleDTO>.InternalServerError();
            }
        }

        public Task<OperationResult<PagedResult<SaleDTO>>> GetAllSale(SaleQueryObject query)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<SaleDetailDTO?>> GetSaleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<SaleDetailDTO?>> GetSaleByInvoiceAsync(string invoice)
        {
            throw new NotImplementedException();
        }

        private static StockAllocationResult AllocateStocksFIFO(
            List<CreateSaleItemDTO> items,
            Dictionary<int, List<Stock>> stockDict
        )
        {
            var outOfStocks = new HashSet<string>();
            var saleItems = new List<SaleItem>();
            var adjustQtyList = new List<AdjustStockDTO>();

            foreach (var item in items)
            {
                if (item.Qty <= 0)
                {
                    return StockAllocationResult.ErrorResult("Qty must be greater than zero");
                }

                if (!stockDict.TryGetValue(item.ProductId, out var productStocks))
                {
                    outOfStocks.Add(item.ProductId.ToString());
                    continue;
                }

                decimal totalPrice = 0;
                int qtyRemaining = item.Qty;
                var tempId = Guid.NewGuid();

                foreach (var stock in productStocks)
                {
                    if (qtyRemaining <= 0)
                        break;

                    var deductQty = Math.Min(stock.QtyRemaining, qtyRemaining);

                    totalPrice += deductQty * stock.PurchasePrice;

                    adjustQtyList.Add(stock.ToAdjustStockDTO(deductQty, tempId));

                    stock.QtyRemaining -= deductQty;

                    qtyRemaining -= deductQty;
                }

                if (qtyRemaining > 0)
                {
                    outOfStocks.Add(item.ProductId.ToString());
                    continue;
                }

                saleItems.Add(item.ToSaleItemFromCreate(totalPrice, tempId));
            }

            if (outOfStocks.Count > 0)
            {
                return StockAllocationResult.ErrorResult(
                    "Insufficient stocks for products: " + string.Join(", ", outOfStocks)
                );
            }

            return StockAllocationResult.SuccessResult(saleItems, adjustQtyList);
        }
    }
}