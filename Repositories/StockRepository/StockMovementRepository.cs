using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.StockInterface;
using FifoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Repositories.StockRepository
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly ApplicationDBContext _context;
        public StockMovementRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<StockMovement>> CreateStockMovementsAsync(List<StockMovement> stockMovements)
        {
            await _context.StockMovements.AddRangeAsync(stockMovements);
            await _context.SaveChangesAsync();
            return stockMovements;
        }

        public Task<List<StockMovement>> DeleteStockMovementListAsync(List<int> saleItemid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsBySaleIdAsync(int SaleId)
        {
            return await _context.StockMovements
                .AnyAsync(s => s.SaleItem.SaleId == SaleId);
        }

        public Task<IQueryable<StockMovement>> GetAllStockMovementAsync(int stockId, StockMovementQueryObject query)
        {
            throw new NotImplementedException();
        }

        public Task<StockMovement?> GetStockMovementByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}