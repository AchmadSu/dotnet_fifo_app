using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.ProductMapper
{
    public static class ProductMapper
    {
        public static ProductDTO ToProductDTO(this Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name
            };
        }
    }
}