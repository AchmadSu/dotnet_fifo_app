using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Helpers.ProductHelper;
using FifoApi.Models;

namespace FifoApi.Interface.ProductInterface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductAsync(ProductQueryObject queryObject);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductBySKUAsync(string sku);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, UpdateProductDTO updateProductDTO);
        Task<Product?> DeleteProductAsync(int id);
        Task<bool> IsExistSKUAsync(string sku);
        Task<bool> IsExistProductNameAsync(string name);
        Task<int> GetNextSkuSequenceAsync(string prefix);
    }
}