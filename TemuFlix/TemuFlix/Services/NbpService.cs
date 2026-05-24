using System.Text.Json;
using TemuFlix.Models;

namespace TemuFlix.Services
{
    public class NbpService
    {
        private readonly HttpClient _http;

        public NbpService(HttpClient http)
        {
            _http = http;
        }

        public async Task<NbpRate?> GetUsdRateAsync()
        {
            try
            {
                var response = await _http.GetAsync(
                    "api/exchangerates/rates/A/USD?format=json");

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NbpResponse>(content);

                return result?.Rates.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<NbpRate?> GetRateAsync(string currencyCode)
        {
            try
            {
                var response = await _http.GetAsync(
                    $"api/exchangerates/rates/A/{currencyCode.ToUpper()}?format=json");

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<NbpResponse>(content);

                return result?.Rates.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}