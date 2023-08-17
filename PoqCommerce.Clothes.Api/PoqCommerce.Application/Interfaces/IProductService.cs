using PoqCommerce.Application.Models.DTOs;

namespace PoqCommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<FilteredProductsDto> FilterProducts(double? minprice, double? maxprice, string size, string highlight);
    }
}
