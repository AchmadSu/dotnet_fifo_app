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
        public static AdjustStockDTO ToAdjustStockDTO(this Stock stock, int qty, Guid tempId)
        {
            return new AdjustStockDTO
            {
                StockId = stock.Id,
                ProductId = stock.ProductId,
                Qty = qty,
                CostPrice = stock.PurchasePrice,
                TempSaleItemId = tempId
            };
        }

        public static AdjustListStockQtyDTO ToAdjustListStockQtyDTO(this List<AdjustStockDTO> data)
        {
            var result = new AdjustListStockQtyDTO();
            foreach (var item in data)
            {
                result.AdjustStockDTOs.Add(new AdjustStockDTO
                {
                    StockId = item.StockId,
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    CostPrice = item.CostPrice,
                    TempSaleItemId = item.TempSaleItemId
                });
            }
            return result;
        }

        public static List<StockMovement> ToStockMovements(this Sale sale, List<AdjustStockDTO> allocation)
        {
            var stockMovements = new List<StockMovement>();
            foreach (var saleItem in sale.SaleItems)
            {
                var itemAllocations = allocation
                    .Where(x => x.TempSaleItemId == saleItem.TempId);

                foreach (var alloc in itemAllocations)
                {
                    stockMovements.Add(new StockMovement
                    {
                        StockBatchId = alloc.StockId,
                        SaleItemId = saleItem.Id,
                        QtyOut = alloc.Qty,
                        CostPrice = alloc.CostPrice
                    });
                }

            }
            return stockMovements;
        }
    }
}