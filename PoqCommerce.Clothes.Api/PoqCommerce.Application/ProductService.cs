using PoqCommerce.Application.DTOs;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Domain;

namespace PoqCommerce.Application
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Title = "A Red Trouser", Price = 10, Sizes = new List<string> { "small", "medium", "large" }, Description = "This trouser perfectly pairs with a green shirt." },
            new Product { Title = "A Red Trouser", Price = 50, Sizes = new List<string> { "small", }, Description = "This trouser perfectly pairs with a red shirt." },
            new Product { Title = "A Red Trouser", Price = 5, Sizes = new List<string> {  "large" }, Description = "This trouser perfectly pairs with a blue shirt." },
        };

        public object FilterProducts(double? minprice, double? maxprice, string size, string highlight)
        {
            var filteredProducts = _products;

            if (minprice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price >= minprice.Value).ToList();

            if (maxprice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price <= maxprice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(size))
                filteredProducts = filteredProducts.Where(p => p.Sizes.Contains(size)).ToList();

            var filterObject = new FilterObject
            {
                MinPrice = _products.Min(p => p.Price),
                MaxPrice = _products.Max(p => p.Price),
                Sizes = _products.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = GetCommonWords()
            };

            foreach (var product in filteredProducts)
            {
                if (!string.IsNullOrWhiteSpace(highlight))
                {
                    var highlightedDescription = HighlightWords(product.Description, highlight);
                    product.Description = highlightedDescription;
                }
            }
            var response = new FilteredProductsDto
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

            return response;
        }

        private List<string> GetCommonWords()
        {
            var wordFrequency = new Dictionary<string, int>();

            foreach (var product in _products)
            {
                var words = product.Description.ToLower().Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
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

            var mostCommonWords = wordFrequency.OrderByDescending(kv => kv.Value)
                                              .Skip(5) // Exclude the most common five
                                              .Take(10) // Take the next ten
                                              .Select(kv => kv.Key)
                                              .ToList();

            return mostCommonWords;
        }

        private string HighlightWords(string description, string highlight)
        {
            var highlightedWords = highlight.Split(',').Select(h => h.Trim().ToLower()).ToList();
            foreach (var highlightedWord in highlightedWords)
            {
                description = description.Replace(highlightedWord, $"<em>{highlightedWord}</em>");
            }

            return description;
        }

    }
}