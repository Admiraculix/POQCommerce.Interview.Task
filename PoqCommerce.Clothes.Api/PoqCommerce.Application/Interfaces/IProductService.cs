namespace PoqCommerce.Application.Interfaces
{
    public interface IProductService
    {
        object FilterProducts(double? minprice, double? maxprice, string size, string highlight);
    }
}
