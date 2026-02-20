using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.ProductDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? SKU { get; set; }
        public string? Name { get; set; }
        // public List<StockBatchesDTO> StockBatches { get; set; }
    }
}