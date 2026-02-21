using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Helpers;

namespace FifoApi.Models
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Only takes 5 to 50 length characters for Product Name")]
        public string SKU { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Only takes 1 to 100 length characters for Product Name")]
        public string Name { get; set; } = string.Empty;
        public List<Stock> StockBatches { get; set; } = new List<Stock>();
        public void NormalizeSKU() => SKU = StringHelper.NormalizeSku(SKU);
    }
}