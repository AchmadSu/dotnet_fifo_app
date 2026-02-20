using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Helpers.ProductHelper;

namespace FifoApi.Interface.ProductInterface
{
    public interface IProductService
    {
        Task<OperationResult<ProductDTO>> CreateAsync(CreateProductDTO productDTO);
        Task<OperationResult<PagedResult<ProductDTO>>> GetAllProductAsync(ProductQueryObject queryObject);
    }
}