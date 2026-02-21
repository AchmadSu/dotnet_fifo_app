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
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}