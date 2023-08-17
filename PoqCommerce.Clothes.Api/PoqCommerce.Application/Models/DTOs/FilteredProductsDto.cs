using PoqCommerce.Domain;

namespace PoqCommerce.Application.Models.DTOs
{
    public class FilteredProductsDto
    {
        public List<Product> Products { get; set; }
        public FilterObjectDto Filter { get; set; }
    }
}
