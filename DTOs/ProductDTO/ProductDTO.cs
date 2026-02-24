using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FifoApi.DTOs.ProductDTO
{
    public class ProductDTO
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        public string? SKU { get; set; }

        [JsonPropertyOrder(3)]
        public string? Name { get; set; }
    }
}