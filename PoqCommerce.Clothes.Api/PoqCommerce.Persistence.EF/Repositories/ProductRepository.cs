using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Persistance.Abstractions;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Domain;

namespace PoqCommerce.Persistence.EF.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(EfDbContext context)
            : base(context)
        {
        }

        public void BulkInsert(IEnumerable<Product> products)
        {
             _context.BulkInsert(products);
        }
    }
}
