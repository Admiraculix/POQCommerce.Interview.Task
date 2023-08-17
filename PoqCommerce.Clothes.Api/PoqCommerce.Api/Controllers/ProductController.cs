using Microsoft.AspNetCore.Mvc;
using PoqCommerce.Application.Interfaces;

namespace PoqCommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
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

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilterProducts(
            [FromQuery] double? minprice = null,
            [FromQuery] double? maxprice = null,
            [FromQuery] string size = null,
            [FromQuery] string highlight = null)
        {
            var result = await _productService.FilterProducts(minprice, maxprice, size, highlight);
            return Ok(result);
        }
    }
}