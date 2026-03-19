using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.SaleDTO;

namespace FifoApi.Helpers.SaleHelper
{
    public class SaleHelper
    {
        public static List<CreateSaleItemDTO> MergeItems(List<CreateSaleItemDTO> items)
        {
            return items
                .GroupBy(x => x.ProductId)
                .Select(g => new CreateSaleItemDTO
                {
                    ProductId = g.Key,
                    Qty = g.Sum(x => x.Qty)
                })
                .ToList();
        }
    }
}