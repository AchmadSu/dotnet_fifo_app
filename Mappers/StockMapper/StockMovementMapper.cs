using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.StockMapper
{
    public static class StockMovementMapper
    {
        public static StockMovementDTO ToMovementDTO(this StockMovement movement)
        {
            return new StockMovementDTO
            {
                Id = movement.Id,
                QtyOut = movement.QtyOut,
                CostPrice = movement.CostPrice
            };
        }
    }
}