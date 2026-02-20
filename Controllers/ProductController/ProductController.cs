using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.ProductDTO;
using FifoApi.Extensions.Controllers;
using FifoApi.Helpers.ProductHelper;
using FifoApi.Interface.ProductInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FifoApi.Controllers.ProductController
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger
        )
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO productDTO)
        {
            try
            {
                var result = await _productService.CreateAsync(productDTO);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while create product {Name}", productDTO.Name);
                return this.ToErrorActionResult();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryObject queryObject)
        {
            try
            {
                var result = await _productService.GetAllProductAsync(queryObject);
                return this.ToActionResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while getting products");
                return this.ToErrorActionResult();
            }
        }
    }
}