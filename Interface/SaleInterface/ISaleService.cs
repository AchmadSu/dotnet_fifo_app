using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.SaleDTO;
using FifoApi.Helpers.SaleHelper;

namespace FifoApi.Interface.SaleInterface
{
    public interface ISaleService
    {
        Task<OperationResult<PagedResult<SaleDTO>>> GetAllSaleAsync(SaleQueryObject query);
        Task<OperationResult<SaleDetailDTO?>> GetSaleByIdAsync(int id);
        Task<OperationResult<SaleDetailDTO?>> GetSaleByInvoiceAsync(string invoice);
        Task<OperationResult<SaleDTO>> CreateSaleAsync(CreateSaleDTO saleDTO);
        // Task<OperationResult<SaleDTO?>> UpdateSaleAsync(string invoice, UpdateSaleDTO saleDTO);
    }
}