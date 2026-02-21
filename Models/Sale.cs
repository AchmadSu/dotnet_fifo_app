using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Models
{
    [Table("Sales")]
    public class Sale : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Only takes 5 to 50 length characters for invoice number sales")]
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public void CalculateTotal() =>
            TotalAmount = SaleItems.Sum(x => x.Qty * x.SalePrice);
    }
}