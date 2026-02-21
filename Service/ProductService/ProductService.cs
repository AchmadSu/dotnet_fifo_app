using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Extensions;
using FifoApi.Helpers;
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
                var productName = StringHelper.CleanStringName(productDTO.Name);
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
                    Name = productName
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
                var productsQuery = await _productRepo.GetAllProductAsync(queryObject);
                if (productsQuery == null || productsQuery.ToArray().Length == 0)
                {
                    return OperationResult<PagedResult<ProductDTO>>.NotFound("Data not found");
                }
                var pagedResult = await productsQuery.ToPagedResultAsync(
                    queryObject.PageNumber,
                    queryObject.PageSize,
                    p => p.ToProductDTO()
                );
                return OperationResult<PagedResult<ProductDTO>>.Ok(pagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting products");
                return OperationResult<PagedResult<ProductDTO>>.InternalServerError();
            }
        }

        public async Task<OperationResult<ProductDetailDTO>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product == null)
                {
                    return OperationResult<ProductDetailDTO>.NotFound("Data not found");
                }
                var productDTO = product.ToProductDetailDTO();
                return OperationResult<ProductDetailDTO>.Ok(productDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting products");
                return OperationResult<ProductDetailDTO>.InternalServerError();
            }
        }

        public async Task<OperationResult<ProductDetailDTO>> GetBySKUAsync(string sku)
        {
            try
            {
                var normalizeSKU = StringHelper.NormalizeSku(sku);
                var product = await _productRepo.GetBySKUAsync(normalizeSKU);
                if (product == null)
                {
                    return OperationResult<ProductDetailDTO>.NotFound("Data not found");
                }
                var productDTO = product.ToProductDetailDTO();
                return OperationResult<ProductDetailDTO>.Ok(productDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting products");
                return OperationResult<ProductDetailDTO>.InternalServerError();
            }
        }

        public async Task<OperationResult<ProductDTO?>> UpdateAsync(int id, UpdateProductDTO productDTO)
        {
            try
            {
                productDTO.Name = StringHelper.CleanStringName(productDTO.Name);
                var existName = await _productRepo.IsExistProductNameAsync(productDTO.Name);
                if (existName) return OperationResult<ProductDTO?>.BadRequest("Failed to create product", new string[]
                {
                $"{productDTO.Name} has been taken!"
                });

                var updateProduct = await _productRepo.UpdateProductAsync(id, productDTO);

                if (updateProduct == null)
                {
                    return OperationResult<ProductDTO?>.BadRequest("Product to update not found!");
                }

                return OperationResult<ProductDTO?>.Ok(
                    updateProduct.ToProductDTO()
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating product");
                return OperationResult<ProductDTO?>.InternalServerError();
            }
        }
    }
}