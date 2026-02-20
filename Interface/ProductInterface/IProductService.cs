using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.ProductDTO;

namespace FifoApi.Interface.ProductInterface
{
    public interface IProductService
    {
        Task<OperationResult<ProductDTO>> CreateAsync(CreateProductDTO productDTO);
    }
}