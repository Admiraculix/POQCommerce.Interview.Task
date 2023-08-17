using PoqCommerce.Application.Models.Responses;

namespace PoqCommerce.Application.Interfaces
{
    public interface IMockyHttpClient
    {
        Task<MockyProductsResponse> GetAllProductsAsync();
    }
}
