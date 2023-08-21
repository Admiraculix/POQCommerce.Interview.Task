using FakeItEasy;
using FluentAssertions;
using PoqCommerce.Application;
using PoqCommerce.Application.Extensions;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models;
using PoqCommerce.Application.Models.DTOs;
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
            var unitOfWorkFake = A.Fake<IUnitOfWork>();
            var productRepositoryFake = A.Fake<IProductRepository>();
            var dtoFactory = A.Fake<IFilterObjectDtoFactory>();
            A.CallTo(() => unitOfWorkFake.Product).Returns(productRepositoryFake);

            var sut = new ProductService(httpClient,unitOfWorkFake, dtoFactory);
            var filter = new FilterObject { MinPrice = 15, MaxPrice = 25, Size = "medium", Highlight = "red" };
            var filterDto = new FilterObjectDto
            {
                MinPrice = _products.Min(p => p?.Price),
                MaxPrice = _products.Max(p => p?.Price),
                Sizes = _products.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = _products.GetCommonWords()
            };

            var withMediumSizeProducts = _products.Where(x => x.Sizes.Contains("medium")).AsQueryable();
            A.CallTo(() => productRepositoryFake.GetAll()).Returns(_products.AsQueryable());
            A.CallTo(() => productRepositoryFake.GetProductsBySize("medium")).Returns(withMediumSizeProducts);
            A.CallTo(() => dtoFactory.CreateFilterObjectDto()).Returns(filterDto);
            // Act
            var result = await sut.FilterProductsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Products.Should().NotBeNull();
            result.Products.Should().NotBeEmpty();
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
            var unitOfWorkFake = A.Fake<IUnitOfWork>();
            var productRepositoryFake = A.Fake<IProductRepository>();
            var dtoFactory = A.Fake<IFilterObjectDtoFactory>();
            A.CallTo(() => unitOfWorkFake.Product).Returns(productRepositoryFake);

            var sut = new ProductService(httpClient, unitOfWorkFake, dtoFactory);

            var filter = new FilterObject();

            var filterDto = new FilterObjectDto
            {
                MinPrice = _products.Min(p => p?.Price),
                MaxPrice = _products.Max(p => p?.Price),
                Sizes = _products.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = _products.GetCommonWords()
            };

            A.CallTo(() => productRepositoryFake.GetAll()).Returns(_products.AsQueryable());
            A.CallTo(() => dtoFactory.CreateFilterObjectDto()).Returns(filterDto);

            // Act
            var result = await sut.FilterProductsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Products.Should().HaveCount(13);
        }
    }
}