using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Helpers.ProductHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Mappers.Paginate;
using FifoApi.Mappers.ProductMapper;
using FifoApi.Models;

namespace FifoApi.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ILogger<ProductService> _logger;
        public ProductService(
            IProductRepository productRepo,
            ILogger<ProductService> logger
        )
        {
            _productRepo = productRepo;
            _logger = logger;
        }
        public async Task<OperationResult<ProductDTO>> CreateAsync(CreateProductDTO productDTO)
        {
            try
            {
                var productName = ProductNameHelper.CleanProductName(productDTO.Name);
                var nextSequence = await _productRepo.GetNextSkuSequenceAsync("PRD");

                var sku = SkuGenerator.Generate(
                    productName: productName,
                    sequenceNumber: nextSequence
                );

                var existSku = await _productRepo.IsExistSKUAsync(sku);
                if (existSku) return OperationResult<ProductDTO>.InternalServerError("There are some issues while creating the Product data, please try again!");

                var existName = await _productRepo.IsExistProductNameAsync(productName);
                if (existName) return OperationResult<ProductDTO>.BadRequest("Failed to create product", new string[]
                {
                $"{productName} has been taken!"
                });

                var productModel = new Product
                {
                    SKU = sku,
                    Name = ProductNameHelper.CleanProductName(productName)
                };

                var createdProduct = await _productRepo.CreateProductAsync(productModel);

                return OperationResult<ProductDTO>.Ok(
                    createdProduct.ToProductDTO()
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating product");
                return OperationResult<ProductDTO>.InternalServerError();
            }
        }

        public async Task<OperationResult<PagedResult<ProductDTO>>> GetAllProductAsync(ProductQueryObject queryObject)
        {
            try
            {
                var pagedResult = await _productRepo.GetAllProductAsync(queryObject);
                if (pagedResult == null || pagedResult.Items.ToArray().Length == 0)
                {
                    return OperationResult<PagedResult<ProductDTO>>.NotFound("Data not found");
                }
                var dtoPagedResult = pagedResult.Map(p => p.ToProductDTO());
                return OperationResult<PagedResult<ProductDTO>>.Ok(dtoPagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting products");
                return OperationResult<PagedResult<ProductDTO>>.InternalServerError();
            }
        }
    }
}