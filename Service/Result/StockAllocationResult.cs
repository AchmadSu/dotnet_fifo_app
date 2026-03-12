using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Models;

namespace FifoApi.Service.Result
{
    public class StockAllocationResult
    {
        private StockAllocationResult(
            bool success,
            string? errorMessage = null,
            List<SaleItem>? saleItems = default,
            List<AdjustStockDTO>? adjustStocks = default
        )
        {
            Success = success;
            ErrorMessage = errorMessage;
            SaleItems = saleItems ?? [];
            AdjustStocks = adjustStocks ?? [];
        }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public List<SaleItem> SaleItems { get; } = new();
        public List<AdjustStockDTO> AdjustStocks { get; } = new();

        public static StockAllocationResult SuccessResult(
            List<SaleItem>? saleItems = default,
            List<AdjustStockDTO>? adjustStocks = default
        )
        {
            return new StockAllocationResult(true, null, saleItems, adjustStocks);
        }

        public static StockAllocationResult ErrorResult(string message = "Unknown stock allocation FIFO error")
        {
            return new StockAllocationResult(false, message);
        }
    }
}