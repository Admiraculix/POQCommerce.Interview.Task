using Persistance.Abstractions.Interfaces;
using PoqCommerce.Domain;

namespace PoqCommerce.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void BulkInsert(IEnumerable<Product> products);
        IQueryable<Product> GetProductsBySize(string size);
    }
}
