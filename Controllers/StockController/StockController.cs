using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Extensions.Controllers;
using FifoApi.Helpers.StockHelper;
using FifoApi.Interface.StockInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FifoApi.Controllers.StockController
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(
            IStockService stockService,
            ILogger<StockController> logger
        )
        {
            _stockService = stockService;
            _logger = logger;
        }

        [HttpPost("{sku}")]
        [Authorize]
        public async Task<IActionResult> CreateStock([FromRoute] string sku, [FromBody] CreateStockDTO stockDTO)
        {
            try
            {
                var result = await _stockService.CreateStockAsync(sku, stockDTO);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while create stock for {sku}", sku);
                return this.ToErrorActionResult();
            }
        }

        [HttpGet("{sku}")]
        [Authorize]
        public async Task<IActionResult> GetAllStock([FromRoute] string sku, [FromQuery] StockQueryObject query)
        {
            try
            {
                var result = await _stockService.GetAllStock(sku, query);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while get stock for {sku}", sku);
                return this.ToErrorActionResult();
            }
        }
    }
}