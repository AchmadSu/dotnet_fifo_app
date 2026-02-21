using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;

namespace FifoApi.DTOs.ProductDTO
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string? SKU { get; set; }
        public string? Name { get; set; }
        public List<StockDTO> StockBatches { get; set; } = new List<StockDTO>();
    }
}