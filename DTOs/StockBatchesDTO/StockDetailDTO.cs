using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Models;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class StockDetailDTO : StockDTO
    {
        public List<StockMovementDTO> StockMovements { get; set; } = new List<StockMovementDTO>();
    }
}