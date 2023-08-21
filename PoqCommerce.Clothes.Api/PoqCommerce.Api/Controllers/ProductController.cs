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

        [HttpPost("DataSeed")]
        public async Task<IActionResult> CreateSeedAsync()
        {
            try
            {

                var result = await _productService.SeedProductsAsync();
                _logger.LogInformation($"{nameof(CreateSeedAsync)} successfully was seeded data to DB");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateSeedAsync)} failed to seed products: {ex.Message} ;\n {ex.StackTrace}");
                return Problem("Something went wrong! Please try again later.");
            }

        }

        [HttpGet("Filter")]
        public async Task<IActionResult> GetProductsAsync([FromQuery] FilterObjectRequest request)
        {
            try
            {
                var dtoRequest = Mapper.Map<FilterObject>(request);
                var result = await _productService.FilterProductsAsync(dtoRequest);
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