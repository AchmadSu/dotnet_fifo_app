using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers;
using FifoApi.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.StockBatches.Include(m => m.StockMovements).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockDTO stockDTO)
        {
            var existingStock = await GetStockByIdAsync(id);
            if (existingStock == null)
            {
                return null;
            }

            SafeMapper.Map(stockDTO, existingStock);

            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}