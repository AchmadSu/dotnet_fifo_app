using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Validations;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class CreateStockDTO
    {
        [Required]
        [Range(typeof(int), "1", "99999999", ErrorMessage = "Qty must be between 0.00 and 99999999")]
        public int QtyIn { get; set; }

        [Required]
        [DecimalRange(100.00, 9999999999999999.99, ErrorMessage = "Price must be between 100.00 and 9999999999999999.99")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date and time.")]
        public DateTime ReceivedAt { get; set; }
    }
}