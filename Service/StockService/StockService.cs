using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Extensions;
using FifoApi.Helpers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers.StockMapper;
using FifoApi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FifoApi.Service.StockService
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<StockService> _logger;

        public StockService(
            IStockRepository stockRepo,
            IProductRepository productRepo,
            ILogger<StockService> logger
        )
        {
            _stockRepo = stockRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        private async Task<Product?> GetProductBySKUAsync(string productSKU)
        {
            var skuNormalize = StringHelper.NormalizeSku(productSKU);
            var product = await _productRepo.GetBySKUAsync(skuNormalize);
            return product;
        }

        public async Task<OperationResult<StockDTO>> CreateStockAsync(string productSKU, CreateStockDTO stockDTO)
        {
            try
            {
                var product = await GetProductBySKUAsync(productSKU);
                if (product == null) return OperationResult<StockDTO>.BadRequest("Product is not exist!");

                var model = stockDTO.ToStockFromCreateDTO(product.Id);
                var result = await _stockRepo.CreateStockAsync(model);
                return OperationResult<StockDTO>.Ok(result.ToStockDTO());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating product");
                return OperationResult<StockDTO>.InternalServerError();
            }
        }

        public Task<OperationResult<StockDTO?>> DeleteStockAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<PagedResult<StockDTO>>> GetAllStock(string productSKU, StockQueryObject query)
        {
            try
            {
                var product = await GetProductBySKUAsync(productSKU);
                if (product == null) return OperationResult<PagedResult<StockDTO>>.BadRequest("Product is not exist!");

                var stocksQuery = await _stockRepo.GetAllStockAsync(product.Id, query);
                if (stocksQuery == null || stocksQuery.ToArray().Length == 0)
                {
                    return OperationResult<PagedResult<StockDTO>>.NotFound("Data not found");
                }

                var pagedResult = await stocksQuery.ToPagedResultAsync(
                    query.PageNumber,
                    query.PageSize,
                    s => s.ToStockDTO()
                );

                return OperationResult<PagedResult<StockDTO>>.Ok(pagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting stocks");
                return OperationResult<PagedResult<StockDTO>>.InternalServerError();
            }
        }

        public async Task<OperationResult<StockDetailDTO?>> GetStockByIDAsync(int id)
        {
            try
            {
                var stock = await _stockRepo.GetStockByIdAsync(id);
                if (stock == null)
                {
                    return OperationResult<StockDetailDTO?>.NotFound("Data not found");
                }
                var stockDTO = stock.ToStockDetailDTO();
                return OperationResult<StockDetailDTO?>.Ok(stockDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting stock by id");
                return OperationResult<StockDetailDTO?>.InternalServerError();
            }
        }

        public async Task<OperationResult<StockDTO?>> UpdateStockAsync(int id, UpdateStockDTO stockDTO)
        {
            try
            {
                var stock = await _stockRepo.GetStockByIdAsync(id);
                if (stock == null) return OperationResult<StockDTO?>.NotFound("Stock not found");

                var updateStock = await _stockRepo.UpdateStockAsync(id, stockDTO);

                if (updateStock == null)
                {
                    return OperationResult<StockDTO?>.BadRequest("Product to update not found!");
                }

                return OperationResult<StockDTO?>.Ok(
                    updateStock.ToStockDTO()
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating stock");
                return OperationResult<StockDTO?>.InternalServerError();
            }
        }
    }
}