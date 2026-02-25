using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.StockInterface;
using FifoApi.Models;

namespace FifoApi.Repositories.StockRepository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            var model = stock;
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Stock>> GetAllStockAsync(int productId, StockQueryObject query)
        {
            var stocks = _context.StockBatches.AsQueryable();
            stocks = stocks.Where(s => s.ProductId == productId);
            stocks = DynamicFilterBuilder.ApplyFilters(stocks, query);
            return stocks;
        }

        public Task<Stock?> GetStockByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Stock?> UpdateStockAsync(UpdateStockDTO stockDTO)
        {
            throw new NotImplementedException();
        }
    }
}