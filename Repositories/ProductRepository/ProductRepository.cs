using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Extensions;
using FifoApi.Helpers.ProductHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Mappers.ProductMapper;
using FifoApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FifoApi.Mappers;

namespace FifoApi.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(
            ApplicationDBContext context
        )
        {
            _context = context;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var productModel = product;
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Product>> GetAllProductAsync(ProductQueryObject queryObject)
        {
            var products = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.Search))
            {
                products = products.Where(p => EF.Functions.Like(p.Name, $"%{queryObject.Search}%"));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = queryObject.IsDescending ?
                        products.OrderByDescending(p => p.Name) :
                        products.OrderBy(p => p.Name);
                }
            }

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.StockBatches).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetBySKUAsync(string sku)
        {
            return await _context.Products.Include(p => p.StockBatches).FirstOrDefaultAsync(p => p.SKU == sku);
        }

        public async Task<int> GetNextSkuSequenceAsync(string prefix)
        {
            var currentMonth = DateTime.UtcNow.ToString("yyyyMM");

            var lastSku = await _context.Products
                .Where(p => p.SKU.StartsWith($"{prefix}-") &&
                            p.SKU.Contains($"-{currentMonth}-"))
                .OrderByDescending(p => p.SKU)
                .Select(p => p.SKU)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastSku))
            {
                return 1;
            }

            var parts = lastSku.Split("-");
            var lastSequence = int.Parse(parts[^1]);

            return lastSequence + 1;
        }

        public async Task<bool> IsExistProductNameAsync(string name)
        {
            var normalized = name.Trim();
            return await _context.Products.AnyAsync(p => p.Name == normalized);
        }

        public async Task<bool> IsExistSKUAsync(string sku)
        {
            var normalized = sku.Trim();
            return await _context.Products.AnyAsync(p => p.SKU == normalized);
        }

        public async Task<Product?> UpdateProductAsync(int id, UpdateProductDTO updateProductDTO)
        {
            var existingProduct = await GetByIdAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            SafeMapper.Map(updateProductDTO, existingProduct);

            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}