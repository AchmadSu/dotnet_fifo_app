using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.SaleDTO
{
    public class SaleItemDTO
    {
        public int Id { get; set; }
        public string? SKU { get; set; }
        public string? Name { get; set; }
        public int Qty { get; set; }
        public decimal SalePrice { get; set; }
    }
}