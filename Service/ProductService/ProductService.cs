using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Extensions;
using FifoApi.Helpers;
using FifoApi.Helpers.ProductHelper;
using FifoApi.Infrastructure.Cache;
using FifoApi.Interface.CacheInterface;
using FifoApi.Interface.ProductInterface;
using FifoApi.Mappers.Paginate;
using FifoApi.Mappers.ProductMapper;

namespace FifoApi.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICacheService _cache;
        private readonly IProductCacheHelper _productCacheHelper;
        private const string cachePrefix = CacheKeys.Products;
        private readonly ILogger<ProductService> _logger;
        public ProductService(
            IProductRepository productRepo,
            ICacheService cache,
            IProductCacheHelper productCacheHelper,
            ILogger<ProductService> logger
        )
        {
            _productRepo = productRepo;
            _cache = cache;
            _productCacheHelper = productCacheHelper;
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

                var createdProduct = await _productRepo.CreateProductAsync(productDTO.ToProductFromCreateDTO(sku));

                await _productCacheHelper.InvalidateListAsync();

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
                var cacheKey = CacheKeyHelper.GenerateListKey(cachePrefix, queryObject);
                var cached = await _cache.GetTAsync<PagedResult<ProductDTO>>(cacheKey);

                if (cached != null)
                    return OperationResult<PagedResult<ProductDTO>>.Ok(cached);

                var productsQuery = await _productRepo.GetAllProductAsync(queryObject);
                if (productsQuery == null || productsQuery.Count() <= 0)
                {
                    return OperationResult<PagedResult<ProductDTO>>.NotFound("Data not found");
                }
                var pagedResult = await productsQuery.ToPagedResultAsync(
                    queryObject.PageNumber,
                    queryObject.PageSize,
                    p => p.ToProductDTO()
                );

                await _cache.SetAsync(cacheKey, pagedResult, TimeSpan.FromMinutes(10));

                return OperationResult<PagedResult<ProductDTO>>.Ok(pagedResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting products");
                return OperationResult<PagedResult<ProductDTO>>.InternalServerError();
            }
        }

        public async Task<OperationResult<ProductDetailDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var cacheKey = $"{cachePrefix}:detail:{id}";
                var cached = await _cache.GetTAsync<ProductDetailDTO>(cacheKey);
                if (cached != null)
                    return OperationResult<ProductDetailDTO?>.Ok(cached);

                var product = await _productRepo.GetByIdAsync(id);
                if (product == null)
                {
                    return OperationResult<ProductDetailDTO?>.NotFound("Data not found");
                }

                var productDTO = product.ToProductDetailDTO();
                await _cache.SetAsync(cacheKey, productDTO, TimeSpan.FromMinutes(10));
                return OperationResult<ProductDetailDTO?>.Ok(productDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting product by id");
                return OperationResult<ProductDetailDTO?>.InternalServerError();
            }
        }

        public async Task<OperationResult<ProductDetailDTO?>> GetBySKUAsync(string sku)
        {
            try
            {
                var normalizeSKU = StringHelper.NormalizeSku(sku);

                var cacheKey = $"{cachePrefix}:sku:{normalizeSKU}";
                var cached = await _cache.GetTAsync<ProductDetailDTO>(cacheKey);
                if (cached != null)
                    return OperationResult<ProductDetailDTO?>.Ok(cached);

                var product = await _productRepo.GetBySKUAsync(normalizeSKU);
                if (product == null)
                {
                    return OperationResult<ProductDetailDTO?>.NotFound("Data not found");
                }

                var productDTO = product.ToProductDetailDTO();
                await _cache.SetAsync(cacheKey, productDTO, TimeSpan.FromMinutes(10));
                return OperationResult<ProductDetailDTO?>.Ok(productDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting product by sku");
                return OperationResult<ProductDetailDTO?>.InternalServerError();
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

                await _productCacheHelper.InvalidateAllAsync(id, StringHelper.NormalizeSku(updateProduct.SKU));

                return OperationResult<ProductDTO?>.Ok(
                    updateProduct.ToProductDTO()
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating product");
                return OperationResult<ProductDTO?>.InternalServerError();
            }
        }
    }
}