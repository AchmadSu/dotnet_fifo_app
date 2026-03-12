using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.SaleDTO;
using FifoApi.Models;

namespace FifoApi.Mappers.SaleMapper
{
    public static class SaleMapper
    {
        public static Sale ToSaleFromCreate(this CreateSaleDTO saleDTO, string invoice, List<SaleItem> saleItems)
        {
            return new Sale
            {
                InvoiceNo = invoice,
                SaleDate = saleDTO.SaleDate,
                SaleItems = saleItems
            };
        }

        public static SaleItem ToSaleItemFromCreate(this CreateSaleItemDTO saleDTO, decimal totalPrice, Guid tempId)
        {
            if (saleDTO.Qty <= 0)
                throw new InvalidOperationException("Qty must be greater than zero");

            return new SaleItem
            {
                ProductId = saleDTO.ProductId,
                Qty = saleDTO.Qty,
                SalePrice = Math.Round(totalPrice / saleDTO.Qty, 2),
                TempId = tempId
            };
        }

        public static SaleDTO ToSaleDTO(this Sale sale)
        {
            return new SaleDTO
            {
                Id = sale.Id,
                InvoiceNo = sale.InvoiceNo,
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}