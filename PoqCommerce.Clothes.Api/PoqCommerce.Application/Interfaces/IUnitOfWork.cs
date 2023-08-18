namespace PoqCommerce.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductService Product { get; }
        void Save();
    }
}
