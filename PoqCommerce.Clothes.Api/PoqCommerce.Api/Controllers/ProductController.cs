using Microsoft.AspNetCore.Mvc;
using PoqCommerce.Api.Controllers.Base;
using PoqCommerce.Api.Models.Contracts.Requests;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models;

namespace PoqCommerce.Api.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(
            ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> GetProductsAsync([FromQuery] FilterObjectRequest request)
        {
            try
            {
                var dtoRequest = Mapper.Map<FilterObject>(request);
                var result = await _productService.FilterProducts(dtoRequest);
                _logger.LogInformation($"{nameof(GetProductsAsync)} successfully was filtered");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetProductsAsync)} failed to get products: {ex.Message} ;\n {ex.StackTrace}");
                return Problem("Something went wrong! Please try again later.");
            }

        }
    }
}