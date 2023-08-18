using Microsoft.EntityFrameworkCore;
using PoqCommerce.Application.Interfaces;

namespace PoqCommerce.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context, IProductService product)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Product = product;
        }

        public IProductService Product { get; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}