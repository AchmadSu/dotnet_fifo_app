using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.StockMapper
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock stock)
        {
            return new StockDTO
            {
                Id = stock.Id,
                QtyIn = stock.QtyIn,
                QtyRemaining = stock.QtyRemaining,
                PurchasePrice = stock.PurchasePrice,
                ReceivedAt = stock.ReceivedAt,
                ProductId = stock.ProductId
            };
        }
    }
}