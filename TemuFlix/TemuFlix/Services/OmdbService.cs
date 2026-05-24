using System.Text.Json;
using TemuFlix.Models;

namespace TemuFlix.Services
{
    public class OmdbService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OmdbService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Omdb:ApiKey"]!;
        }

        public async Task<OmdbMovie?> SearchByTitleAsync(string title)
        {
            var url = $"?t={Uri.EscapeDataString(title)}&apikey={_apiKey}";

            try
            {
                var response = await _http.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OmdbMovie>(content);

                if (result?.Response == "False") return null;
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<OmdbMovie>> SearchByQueryAsync(string query)
        {
            // OMDB search endpoint zwraca listę wyników
            var url = $"?s={Uri.EscapeDataString(query)}&apikey={_apiKey}";

            try
            {
                var response = await _http.GetAsync(url);
                if (!response.IsSuccessStatusCode) return [];

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                if (json.RootElement.GetProperty("Response").GetString() == "False")
                    return [];

                var results = new List<OmdbMovie>();
                foreach (var item in json.RootElement.GetProperty("Search").EnumerateArray())
                {
                    results.Add(new OmdbMovie
                    {
                        Title = item.GetProperty("Title").GetString() ?? "",
                        Year = item.GetProperty("Year").GetString() ?? "",
                        ImdbId = item.GetProperty("imdbID").GetString() ?? "",
                        Poster = item.GetProperty("Poster").GetString() ?? "",
                    });
                }
                return results;
            }
            catch
            {
                return [];
            }
        }
    }
}