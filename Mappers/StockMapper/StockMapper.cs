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

        public static Stock ToStockFromCreateDTO(this CreateStockDTO stockDTO, int productId)
        {
            return new Stock
            {
                QtyIn = stockDTO.QtyIn,
                QtyRemaining = stockDTO.QtyIn,
                PurchasePrice = stockDTO.PurchasePrice,
                ReceivedAt = stockDTO.ReceivedAt,
                ProductId = productId
            };
        }

        public static StockDetailDTO ToStockDetailDTO(this Stock stock)
        {
            return new StockDetailDTO
            {
                Id = stock.Id,
                QtyIn = stock.QtyIn,
                QtyRemaining = stock.QtyRemaining,
                PurchasePrice = stock.PurchasePrice,
                ReceivedAt = stock.ReceivedAt,
                ProductId = stock.ProductId,
                StockMovements = stock.StockMovements.Select(m => m.ToMovementDTO()).ToList()
            };
        }
    }
}