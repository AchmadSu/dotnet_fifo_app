using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Helpers.StockHelper;

namespace FifoApi.Interface.StockInterface
{
    public interface IStockService
    {
        Task<OperationResult<PagedResult<StockDTO>>> GetAllStock(string productSKU, StockQueryObject query);
        Task<OperationResult<StockDetailDTO?>> GetStockByIDAsync(int id);
        Task<OperationResult<StockDTO>> CreateStockAsync(string productSKU, CreateStockDTO stockDTO);
        Task<OperationResult<StockDTO?>> UpdateStockAsync(int productId, UpdateStockDTO stockDTO);
        Task<OperationResult<StockDTO?>> DeleteStockAsync(int id);
    }
}