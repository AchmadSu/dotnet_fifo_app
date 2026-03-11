using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.StockMapper
{
    public static class StockAdjustMapper
    {
        public static AdjustStockDTO ToAdjustStockDTO(this Stock stock, int qty)
        {
            return new AdjustStockDTO
            {
                Id = stock.Id,
                Qty = qty
            };
        }

        public static AdjustListStockQtyDTO ToAdjustListStockQtyDTO(this List<AdjustStockDTO> data)
        {
            var result = new AdjustListStockQtyDTO();
            foreach (var item in data)
            {
                result.AdjustStockDTOs.Add(new AdjustStockDTO
                {
                    Id = item.Id,
                    Qty = item.Qty
                });
            }
            return result;
        }
    }
}