using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.SaleDTO
{
    public class SaleDetailDTO : SaleDTO
    {
        public List<SaleItemDTO> SaleItems { get; set; } = new List<SaleItemDTO>();
    }
}