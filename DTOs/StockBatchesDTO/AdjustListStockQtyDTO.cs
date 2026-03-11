using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class AdjustListStockQtyDTO
    {
        public List<AdjustStockDTO> AdjustStockDTOs { get; set; } = new List<AdjustStockDTO>();
    }
}