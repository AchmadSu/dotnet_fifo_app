using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Helpers.SaleHelper;
using FifoApi.Models;

namespace FifoApi.Interface.SaleInterface
{
    public interface ISaleRepository
    {
        Task<IQueryable<Sale>> GetAllSalesAsync(SaleQueryObject query);
        Task<Sale?> GetByIdAsync(int id);
        Task<Sale?> GetByInvoiceAsync(string invoice);
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<int> GetNextInvoiceSequenceAsync(string prefix);
    }
}