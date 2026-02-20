using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.ProductDTO
{
    public class UpdateProductDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ID")]
        public int Id { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string? SKU { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }
    }
}