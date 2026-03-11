using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.SaleDTO
{
    public class CreateSaleDTO
    {
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date and time.")]
        public DateTime SaleDate { get; set; }
        [Required]
        public List<CreateSaleItemDTO> Items { get; set; } = new List<CreateSaleItemDTO>();
    }
}