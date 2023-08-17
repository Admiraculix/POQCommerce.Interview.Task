using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Application.Models.Responses;
using PoqCommerce.Mocky.Io.Configurations;

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
            MockyProductsResponse responseContent = null;
            try
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
                    _logger.LogError($"{nameof(GetAllProductsAsync)} failed to get all products: {responseContentString}");
                    throw new HttpRequestException();
                }

                responseContent = JsonConvert.DeserializeObject<MockyProductsResponse>(responseContentString);
                _logger.LogInformation($"{nameof(GetAllProductsAsync)} successfully get all product data!");

                return responseContent;
            }
            catch (Exception ex)
            {
               _logger.LogError($"{nameof(GetAllProductsAsync)} failed to get all products: {ex.Message} ;\n {ex.StackTrace}");
                return responseContent;
            }
        }
    }
}