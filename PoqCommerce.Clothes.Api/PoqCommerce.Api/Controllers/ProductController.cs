using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoqCommerce.Api.Models.Contracts.Requests;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models;

namespace PoqCommerce.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();

        public ProductController(
            ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilterProducts([FromQuery] FilterObjectRequest request)
        {
            try
            {
                var dtoRequest = Mapper.Map<FilterObject>(request);
                var result = await _productService.FilterProducts(dtoRequest);
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