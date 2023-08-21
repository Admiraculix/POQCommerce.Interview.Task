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

        public ProductService(IMockyHttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
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
                //query = query.Where(p => p.Sizes.Contains(filter.Size));
                query =  _unitOfWork.Product.GetProductsBySize(filter.Size).AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(filter.Highlight))
                query = ApplyHighlight(query, filter.Highlight);

            var filteredProducts = query.ToList();

            var filterObject = new FilterObjectDto
            {
                MinPrice = filteredProducts.Min(p => p?.Price),
                MaxPrice = filteredProducts.Max(p => p?.Price),
                Sizes = filteredProducts.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = GetCommonWords(filteredProducts)
            };

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
                Filter = filterObject
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

        private List<string> GetCommonWords(List<Product> products)
        {
            var wordFrequency = new Dictionary<string, int>();

            foreach (var product in products)
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