using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Mappers;
using FifoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Repositories.StockRepository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        // private readonly IProductRepository _productRepo;
        public StockRepository(
            ApplicationDBContext context
        // IProductRepository productRepo
        )
        {
            _context = context;
            // _productRepo = productRepo;
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

        public async Task<List<Stock>> GetAvailableStockAsync(List<int> productIds)
        {
            productIds = productIds.Distinct().ToList();
            return await _context.StockBatches
                .FromSqlInterpolated($@"
                    SELECT *
                    FROM ""StockBatches""
                    WHERE ""ProductId"" = ANY({productIds})
                    AND ""QtyRemaining"" > 0
                    ORDER BY ""ProductId"", ""ReceivedAt""
                    FOR UPDATE SKIP LOCKED
                ")
                .ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.StockBatches
                .Include(s => s.StockMovements)
                .ThenInclude(m => m.SaleItem)
                .ThenInclude(i => i.Sale)
                .FirstOrDefaultAsync(s => s.Id == id);
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

        private static Task<bool> ValidateAdjustOperator(string opr)
        {
            return Task.FromResult(opr == "+" || opr == "-");
        }

        public async Task<bool> AdjustStockQtyAsync(int id, string opr, int qty = 0)
        {
            if (!await ValidateAdjustOperator(opr))
                return false;

            var stock = await GetStockByIdAsync(id);
            if (stock == null)
                return false;

            if (opr == "-" && stock.QtyRemaining < qty)
                return false;

            stock.QtyRemaining = opr == "+"
                ? stock.QtyRemaining + qty
                : stock.QtyRemaining - qty;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AdjustListStockQtyAsync(AdjustListStockQtyDTO adjustDTO, string opr)
        {
            if (!await ValidateAdjustOperator(opr))
                return false;

            var data = adjustDTO.AdjustStockDTOs;
            if (data == null || data.Count == 0)
                return false;

            var ids = data.Select(x => x.StockId).Distinct().ToList();

            var existingIds = await _context.StockBatches
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (existingIds.Count != ids.Count)
                return false;

            var sql = new StringBuilder();
            sql.Append($"UPDATE \"StockBatches\" SET \"QtyRemaining\" = \"QtyRemaining\" {opr} CASE ");

            var parameters = new List<object>();
            int i = 0;

            foreach (var item in data)
            {
                sql.Append($"WHEN \"Id\" = @id{i} THEN @qty{i} ");

                parameters.Add(new Npgsql.NpgsqlParameter($"id{i}", item.StockId));
                parameters.Add(new Npgsql.NpgsqlParameter($"qty{i}", item.Qty));

                i++;
            }

            sql.Append("END ");
            sql.Append($"WHERE \"Id\" IN (");
            for (int j = 0; j < ids.Count; j++)
            {
                if (j > 0) sql.Append(", ");
                sql.Append($"@wid{j}");
                parameters.Add(new Npgsql.NpgsqlParameter($"wid{j}", ids[j]));
            }
            sql.Append(")");
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters);

            if (affectedRows != data.Count)
                return false;

            return true;
        }

        public async Task<int> GetGrandTotalStockAsync(int productId)
        {
            return await _context.StockBatches
            .AsNoTracking()
            .Where(s => s.ProductId == productId && s.QtyRemaining > 0)
            .SumAsync(
                s => s.QtyRemaining
            );
        }
    }
}