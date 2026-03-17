using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.SaleDTO
{
    public class CreateSaleItemDTO
    {
        [Required]
        [Range(typeof(int), "1", "99999999", ErrorMessage = "Invalid Product Id")]
        public int ProductId { get; set; }
        [Required]
        [Range(typeof(int), "1", "99999999", ErrorMessage = "Qty must be between 1 and 99999999")]
        public int Qty { get; set; }
    }
}