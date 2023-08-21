using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models;
using PoqCommerce.Application.Models.DTOs;
using PoqCommerce.Domain;
using System.Text.RegularExpressions;

namespace PoqCommerce.Application
{
    public class ProductService : IProductService
    {
        private readonly IMockyHttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilterObjectDtoFactory _filterObjectDtoFactory;

        public ProductService(
            IMockyHttpClient httpClient,
            IUnitOfWork unitOfWork,
            IFilterObjectDtoFactory filterObjectDtoFactory)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _filterObjectDtoFactory = filterObjectDtoFactory;
        }

        public async Task<FilteredProductsDto> FilterProductsAsync(FilterObject filter)
        {
            var query = _unitOfWork.Product.GetAll();

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.Size))
            {
                var sizeQuery = _unitOfWork.Product.GetProductsBySize(filter.Size).AsQueryable();
                query = query.Intersect(sizeQuery);
            }

            if (!string.IsNullOrWhiteSpace(filter.Highlight))
                query = ApplyHighlight(query, filter.Highlight);

            var filteredProducts = query.ToList();

            var result = new FilteredProductsDto
            {
                Products = filteredProducts.Select(p => new Product
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Sizes = p.Sizes,
                    Description = p.Description
                }).ToList(),
                Filter = _filterObjectDtoFactory.CreateFilterObjectDto()
            };

            return result;
        }

        public async Task<SeedResultDto> SeedProductsAsync()
        {
            var enitites = _unitOfWork.Product.GetAll();
            if (enitites.Any())
            {
                return new SeedResultDto { Count = enitites.Count() };
            }

            var response = await _httpClient.GetAllProductsAsync();
            var products = response.Products.ToList();

            _unitOfWork.Product.BulkInsert(products);
            _unitOfWork.Commit();
            var result = new SeedResultDto { Count = products.Count };

            return result;
        }

        private IQueryable<Product> ApplyHighlight(IQueryable<Product> products, string highlight)
        {
            var highlightedWords = highlight.Split(',').Select(h => h.Trim().ToLower()).ToList();

            foreach (var highlightedWord in highlightedWords)
            {
                products = products.Select(p => new Product
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description.Replace(highlightedWord, $"<em>{highlightedWord}</em>", StringComparison.OrdinalIgnoreCase),
                    Price = p.Price,
                    Sizes = p.Sizes
                });
            }

            return products;
        }
    }
}