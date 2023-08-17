using Microsoft.Extensions.Logging;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Mocky.Io.Configurations;
using PoqCommerce.Application.Models.Responses;
using Newtonsoft.Json;

namespace PoqCommerce.Mocky.Io
{
    public class MockyHttpClient : IMockyHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly MockyClientConfiguration _config;
        private readonly ILogger<MockyHttpClient> _logger;

        public MockyHttpClient(HttpClient httpClient, MockyClientConfiguration config, ILogger<MockyHttpClient> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<MockyProductsResponse> GetAllProductsAsync()
        {
            using var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{_config.ProductUrl}"),
            };

            using var response = await _httpClient.SendAsync(httpRequest);
            var responseContentString = await response.Content?.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var responseContent = JsonConvert.DeserializeObject<MockyProductsResponse>(responseContentString);

            return responseContent;
        }

    }
}