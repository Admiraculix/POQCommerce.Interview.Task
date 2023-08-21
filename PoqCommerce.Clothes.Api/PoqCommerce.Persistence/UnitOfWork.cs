using Microsoft.EntityFrameworkCore;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Persistence.EF;

namespace PoqCommerce.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _dbContext;
        private bool _disposed = false;

        public UnitOfWork(EfDbContext context, IProductRepository product)
        {
            _dbContext = context;
            Product = product;
        }

        public IProductRepository Product { get; }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}