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
            List<AdjustStockDTO>? adjustStocks = default,
            HashSet<int>? outOfStockIds = default
        )
        {
            Success = success;
            ErrorMessage = errorMessage;
            SaleItems = saleItems ?? [];
            AdjustStocks = adjustStocks ?? [];
            OutOfStockIds = outOfStockIds ?? [];
        }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public HashSet<int> OutOfStockIds { get; set; } = new();

        public List<SaleItem> SaleItems { get; } = new();
        public List<AdjustStockDTO> AdjustStocks { get; } = new();

        public static StockAllocationResult SuccessResult(
            List<SaleItem>? saleItems = default,
            List<AdjustStockDTO>? adjustStocks = default
        )
        {
            return new StockAllocationResult(true, null, saleItems, adjustStocks);
        }

        public static StockAllocationResult ErrorResult(
            string message = "Unknown stock allocation FIFO error",
            HashSet<int>? outOfStockIds = default
        )
        {
            return new StockAllocationResult(false, message, null, null, outOfStockIds);
        }
    }
}