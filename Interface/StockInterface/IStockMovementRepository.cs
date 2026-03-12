using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Helpers.StockHelper;
using FifoApi.Models;

namespace FifoApi.Interface.StockInterface
{
    public interface IStockMovementRepository
    {
        Task<IQueryable<StockMovement>> GetAllStockMovementAsync(int stockId, StockMovementQueryObject query);
        Task<StockMovement?> GetStockMovementByIdAsync(int id);
        Task<List<StockMovement>> CreateStockMovementsAsync(List<StockMovement> stockMovements);
        Task<List<StockMovement>> DeleteStockMovementListAsync(List<int> saleItemid);
    }
}