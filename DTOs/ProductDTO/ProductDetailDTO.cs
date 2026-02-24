using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;

namespace FifoApi.DTOs.ProductDTO
{
    public class ProductDetailDTO : ProductDTO
    {
        [JsonPropertyOrder(4)]
        public List<StockDTO> StockBatches { get; set; } = new List<StockDTO>();
    }
}