using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Models
{
    [Table("SaleItems")]
    public class SaleItem : BaseEntity
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}