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
        private List<Product> _products = new List<Product>();
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IMockyHttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<FilteredProductsDto> FilterProductsAsync(FilterObject filter)
        {
            var response = await _httpClient.GetAllProductsAsync();
            _products = response.Products.ToList();

            var filteredProducts = _products;

            if (filter.MinPrice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price >= filter.MinPrice.Value).ToList();

            if (filter.MaxPrice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price <= filter.MaxPrice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Size))
                filteredProducts = filteredProducts.Where(p => p.Sizes.Contains(filter.Size)).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Highlight))
                filteredProducts = ApplyHighlight(filteredProducts, filter.Highlight);

            var filterObject = new FilterObjectDto
            {
                MinPrice = _products.Min(p => p.Price),
                MaxPrice = _products.Max(p => p.Price),
                Sizes = _products.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = GetCommonWords()
            };

            var result = new FilteredProductsDto
            {
                Products = filteredProducts.Select(p => new Product
                {
                    Title = p.Title,
                    Price = p.Price,
                    Sizes = p.Sizes,
                    Description = p.Description
                }).ToList(),
                Filter = filterObject
            };

            return result;
        }

        public async Task<SeedResultDto> SeedProductsAsync()
        {
            var enitites = _unitOfWork.Product.GetAll();
            if (enitites.Any())
            {
                return new SeedResultDto { Count  = enitites.Count()} ;
            }

            var response = await _httpClient.GetAllProductsAsync();
            _products = response.Products.ToList();

            _unitOfWork.Product.BulkInsert(_products);
            _unitOfWork.Commit();
            var result = new SeedResultDto { Count = _products.Count };

            return result;
        }

        private List<Product> ApplyHighlight(List<Product> products, string highlight)
        {
            var highlightedWords = highlight.Split(',').Select(h => h.Trim().ToLower()).ToList();

            foreach (var product in products)
            {
                foreach (var highlightedWord in highlightedWords)
                {
                    product.Description = product.Description.Replace(highlightedWord, $"<em>{highlightedWord}</em>", StringComparison.OrdinalIgnoreCase);
                }
            }

            return products;
        }

        private List<string> GetCommonWords()
        {
            var wordFrequency = new Dictionary<string, int>();

            foreach (var product in _products)
            {
                var descriptionWithoutTags = StripHtmlTags(product.Description.ToLower());
                var words = descriptionWithoutTags.Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    if (word.Length > 2) // Exclude words with less than three characters
                    {
                        if (!wordFrequency.ContainsKey(word))
                        {
                            wordFrequency[word] = 1;
                        }
                        else
                        {
                            wordFrequency[word]++;
                        }
                    }
                }
            }

            var mostCommonWords = wordFrequency.OrderByDescending(kv => kv.Value)
                                              .Skip(5) // Exclude the most common five
                                              .Take(10) // Take the next ten
                                              .Select(kv => kv.Key)
                                              .ToList();

            return mostCommonWords;
        }

        private string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}