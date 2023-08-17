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
            try
            {
                var result = await _productService.FilterProducts(minprice, maxprice, size, highlight);
                _logger.LogInformation($"{nameof(GetFilterProducts)} successfully was filtered");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetFilterProducts)} failed to get products: {ex.Message} ;\n {ex.StackTrace}");
                return Problem("Something went wrong! Please try again later.");
            }

        }
    }
}