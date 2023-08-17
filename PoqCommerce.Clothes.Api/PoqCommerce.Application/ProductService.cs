using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models.DTOs;
using PoqCommerce.Domain;
using System.Text.RegularExpressions;

namespace PoqCommerce.Application
{
    public class ProductService : IProductService
    {
        private readonly  IMockyHttpClient _httpClient;
        private List<Product> _products = new List<Product>
        {
            new Product { Title = "A Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium", "large" }, Description = "This trouser perfectly pairs with a green shirt." },
            new Product { Title = "A Red Trouser", Price = 50, Sizes = new List<string> { "small", }, Description = "This trouser perfectly pairs with a red shirt." },
            new Product { Title = "A Red Trouser", Price = 5, Sizes = new List<string> {  "large" }, Description = "This trouser perfectly pairs with a yellow shirt." },
            new Product { Title = "A Red Trouser", Price = 1, Sizes = new List<string> { "medium", "large" }, Description = "This trouser perfectly pairs with a blue shirt." },
            new Product { Title = "A Blue Trouser", Price = 8, Sizes = new List<string> { "small", "medium", "large" }, Description = "This trouser perfectly pairs with a orange shirt." },
            new Product { Title = "A Blue Trouser", Price = 55, Sizes = new List<string> { "small", }, Description = "This trouser perfectly pairs with a red shirt." },
            new Product { Title = "A Blue Trouser", Price = 5, Sizes = new List<string> {  "medium", "large" }, Description = "This trouser perfectly pairs with a light blue shirt." },
            new Product { Title = "A Blue Trouser", Price = 10, Sizes = new List<string> { "small", "medium", "large" }, Description = "This trouser perfectly pairs with a green shirt." },
            new Product { Title = "A Green Trouser", Price = 150, Sizes = new List<string> { "small", }, Description = "This trouser perfectly pairs with a dark red shirt." },
            new Product { Title = "A Green Trouser", Price = 7, Sizes = new List<string> { "medium", "large" }, Description = "This trouser perfectly pairs with a magenta shirt." },
            new Product { Title = "A Green Trouser", Price = 70, Sizes = new List<string> { "small", "medium", "large" }, Description = "This trouser perfectly pairs with a cyan shirt." },
            new Product { Title = "A Green Trouser", Price = 20, Sizes = new List<string> { "small", }, Description = "This trouser perfectly pairs with a black shirt." },
            new Product { Title = "A Orange Trouser", Price = 25, Sizes = new List<string> { "medium", }, Description = "This trouser perfectly pairs with a white shirt." },
        };

        public ProductService(IMockyHttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<FilteredProductsDto> FilterProducts(double? minprice, double? maxprice, string size, string highlight)
        {
            var response = await _httpClient.GetAllProductsAsync();
            _products = response.Products.ToList();

            var filteredProducts = _products;

            if (minprice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price >= minprice.Value).ToList();

            if (maxprice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price <= maxprice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(size))
                filteredProducts = filteredProducts.Where(p => p.Sizes.Contains(size)).ToList();

            if (!string.IsNullOrWhiteSpace(highlight))
                filteredProducts = ApplyHighlight(filteredProducts, highlight);

            var filterObject = new FilterObject
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