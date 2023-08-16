using PoqCommerce.Domain;

namespace PoqCommerce.Application.DTOs
{
    public class FilteredProductsDto
    {
        public List<Product> Products { get; set; }
        public FilterObject Filter { get; set; }
    }
}
