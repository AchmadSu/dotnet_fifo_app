using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.SaleDTO
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string? InvoiceNo { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}