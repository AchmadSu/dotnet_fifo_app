using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Data;
using FifoApi.Helpers.SaleHelper;
using FifoApi.Interface.SaleInterface;
using FifoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Repositories.SaleRepository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDBContext _context;
        public SaleRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            var model = sale;
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public Task<IQueryable<Sale>> GetAllSalesAsync(SaleQueryObject query)
        {
            throw new NotImplementedException();
        }

        public Task<Sale?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Sale?> GetByInvoiceAsync(string invoice)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetNextInvoiceSequenceAsync(string prefix)
        {
            var currentMonth = DateTime.UtcNow.ToString("yyyyMM");

            var lastInvoice = await _context.Sales
                .Where(s => s.InvoiceNo.StartsWith($"{prefix}-") &&
                            s.InvoiceNo.Contains($"-{currentMonth}-"))
                .OrderByDescending(s => s.InvoiceNo)
                .Select(s => s.InvoiceNo)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastInvoice))
            {
                return 1;
            }

            var parts = lastInvoice.Split("-");
            var lastSequence = int.Parse(parts[^1]);

            return lastSequence + 1;
        }
    }
}