using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers.StockHelper;
using FifoApi.Models;

namespace FifoApi.Interface.StockInterface
{
    public interface IStockRepository
    {
        Task<IQueryable<Stock>> GetAllStockAsync(int productId, StockQueryObject query);
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock> CreateStockAsync(Stock stock);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockDTO stockDTO);
        Task<Stock?> DeleteStockAsync(int id);
        Task<List<Stock>> GetAvailableStockAsync(List<int> productIds);
        Task<bool> AdjustStockQtyAsync(int id, string opr, int qty = 0);
        Task<bool> AdjustListStockQtyAsync(AdjustListStockQtyDTO adjustDTO, string opr);
    }
}