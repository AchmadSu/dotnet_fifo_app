using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Validations;
using FifoApi.Validations.Enum;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class UpdateStockDTO
    {
        [Range(typeof(int), "1", "99999999", ErrorMessage = "Price must be between 0.00 and 99999999")]
        public int QtyIn { get; set; }

        [Range(typeof(int), "0", "99999999", ErrorMessage = "Price must be between 0.00 and 99999999")]
        [CompareToProperty(nameof(QtyIn), ComparisonOperatorType.LessThanEqual)]
        public int QtyRemaining { get; set; }

        [DecimalRange(100.00, 9999999999999999.99, ErrorMessage = "Price must be between 100.00 and 9999999999999999.99")]
        public decimal PurchasePrice { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date and time.")]
        public DateTime ReceivedAt { get; set; }
    }
}