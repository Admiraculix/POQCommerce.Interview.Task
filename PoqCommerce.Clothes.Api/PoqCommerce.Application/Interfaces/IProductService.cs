using PoqCommerce.Application.Models;
using PoqCommerce.Application.Models.DTOs;

namespace PoqCommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<FilteredProductsDto> FilterProducts(FilterObject filter);
    }
}
