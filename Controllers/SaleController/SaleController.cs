using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.SaleDTO;
using FifoApi.Extensions.Controllers;
using FifoApi.Helpers.SaleHelper;
using FifoApi.Interface.SaleInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FifoApi.Controllers.SaleController
{
    [Route("api/sales")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(
            ISaleService saleService,
            ILogger<SaleController> logger
        )
        {
            _saleService = saleService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateSaleDTO saleDTO)
        {
            try
            {
                var result = await _saleService.CreateSaleAsync(saleDTO);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while creating sale data");
                return this.ToErrorActionResult();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] SaleQueryObject query)
        {
            try
            {
                var result = await _saleService.GetAllSaleAsync(query);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while getting sales");
                return this.ToErrorActionResult();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _saleService.GetSaleByIdAsync(id);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while getting sale by id");
                return this.ToErrorActionResult();
            }
        }

        [HttpGet("invoice/{invoice}")]
        [Authorize]
        public async Task<IActionResult> GetByInvoice(string invoice)
        {
            try
            {
                var result = await _saleService.GetSaleByInvoiceAsync(invoice);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while getting sale by invoice");
                return this.ToErrorActionResult();
            }
        }
    }
}