using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Models
{
    [Table("StockMovements")]
    public class StockMovement : BaseEntity
    {
        public int Id { get; set; }
        public int StockBatchId { get; set; }
        public Stock StockBatch { get; set; } = null!;
        public int SaleItemId { get; set; }
        public SaleItem SaleItem { get; set; } = null!;
        public int QtyOut { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

    }
}