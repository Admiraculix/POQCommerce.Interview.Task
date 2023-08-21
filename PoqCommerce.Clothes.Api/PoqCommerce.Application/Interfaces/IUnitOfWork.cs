using Microsoft.EntityFrameworkCore;

namespace PoqCommerce.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        public void Rollback();
        IProductRepository Product { get; }
    }
}
