using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers.StockMapper;
using FifoApi.Models;

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

        public async Task<OperationResult<StockDTO>> CreateStockAsync(string productSKU, CreateStockDTO stockDTO)
        {
            try
            {
                var skuNormalize = StringHelper.NormalizeSku(productSKU);
                var product = await _productRepo.GetBySKUAsync(skuNormalize);
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

        public Task<OperationResult<PagedResult<StockDTO>>> GetAllStock(int productId, StockQueryObject query)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StockDetailDTO?>> GetStockByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<StockDTO?>> UpdateStockAsync(int productId, UpdateStockDTO stockDTO)
        {
            throw new NotImplementedException();
        }
    }
}