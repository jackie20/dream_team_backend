using System.Net.Http;
using System.Threading.Tasks;
using Dream_Team_Assessment.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace StockApi.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private const int CacheDurationInMinutes = 5;

        public StockService(HttpClient httpClient, IMemoryCache cache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<string> GetDailyStockPricesAsync(string symbol)
        {
            // Check cache first to limit API calls
            if (_cache.TryGetValue(symbol, out string cachedJson))
                return cachedJson;

            // Build request with required headers and parameters
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://alpha-vantage.p.rapidapi.com/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&datatype=json");

            // Set RapidAPI authentication headers
            request.Headers.Add("X-RapidAPI-Key", _configuration["AlphaVantage:ApiKey"]);
            request.Headers.Add("X-RapidAPI-Host", _configuration["AlphaVantage:ApiHost"]);

            // Send the request and ensure the response is successful
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Read raw JSON response
            var json = await response.Content.ReadAsStringAsync();

            // Cache the result
            _cache.Set(symbol, json, TimeSpan.FromMinutes(CacheDurationInMinutes));

            return json;
        }
    }
}
