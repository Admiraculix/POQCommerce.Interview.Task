using FakeItEasy;
using FluentAssertions;
using PoqCommerce.Application;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models.Responses;
using PoqCommerce.Domain;

namespace PoqCommerce.Unit.Tests.Services
{
    public class ProductServiceTest
    {
        private readonly List<Product> _products = new List<Product>
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

        [Fact]
        public async Task FilterProducts_WithFilters_AppliesFiltersAndReturnsFilteredProducts()
        {
            // Arrange
            var httpClient = A.Fake<IMockyHttpClient>();
            var productService = new ProductService(httpClient);

            var fakeResponse = new MockyProductsResponse
            {
                Products = _products
            };

            A.CallTo(() => httpClient.GetAllProductsAsync()).Returns(fakeResponse);

            // Act
            var result = await productService.FilterProducts(15, 25, "medium", "red");

            // Assert
            result.Should().NotBeNull();
            result.Products.Should().HaveCount(1);
            result.Products[0].Title.Should().Be("A Orange Trouser");
            result.Filter.MinPrice.Should().Be(1);
            result.Filter.MaxPrice.Should().Be(150);
            result.Filter.Sizes.Should().BeEquivalentTo("small", "medium", "large");
            result.Filter.CommonWords.Should().NotBeNull();
        }

        [Fact]
        public async Task FilterProducts_WithoutFilters_ReturnsAllProducts()
        {
            // Arrange
            var httpClient = A.Fake<IMockyHttpClient>();
            var productService = new ProductService(httpClient);

            var fakeResponse = new MockyProductsResponse
            {
                Products = _products
            };

            A.CallTo(() => httpClient.GetAllProductsAsync()).Returns(fakeResponse);

            // Act
            var result = await productService.FilterProducts(null, null, null, null);

            // Assert
            result.Should().NotBeNull();
            result.Products.Should().HaveCount(13);
        }
    }
}