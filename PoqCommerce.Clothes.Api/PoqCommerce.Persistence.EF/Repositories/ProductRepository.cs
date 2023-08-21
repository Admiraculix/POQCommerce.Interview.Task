using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
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

        public IQueryable<Product> GetProductsBySize(string size)
        {
            var sizeParameter = new SqlParameter("@size", System.Data.SqlDbType.NVarChar)
            {
                Value = size
            };

            var query = _dbSet.FromSqlRaw("SELECT * FROM Products WHERE CHARINDEX(@size, Sizes) > 0", sizeParameter);

            return query;
        }
    }
}
