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
        public void ValidateQtyOut()
        {
            if (QtyOut <= 0)
            {
                throw new InvalidOperationException("Qty Out must be greater than 0");
            }
            if (QtyOut > StockBatch.QtyRemaining)
                throw new InvalidOperationException("Qty Out cannot exceed Stock Batch Qty Remaining");
        }
    }
}