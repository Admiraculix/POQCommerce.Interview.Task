using Microsoft.EntityFrameworkCore;
using Persistance.Abstractions;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Domain;

namespace PoqCommerce.Persistence.EF.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }

        // Implement additional methods specific to ProductRepository if needed
    }
}
