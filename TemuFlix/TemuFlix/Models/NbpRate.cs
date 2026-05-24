using System.Text.Json.Serialization;

namespace TemuFlix.Models
{
    public class NbpResponse
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("rates")]
        public List<NbpRate> Rates { get; set; } = [];
    }

    public class NbpRate
    {
        [JsonPropertyName("no")]
        public string No { get; set; } = string.Empty;

        [JsonPropertyName("effectiveDate")]
        public string EffectiveDate { get; set; } = string.Empty;

        [JsonPropertyName("mid")]
        public decimal Mid { get; set; }
    }

    public class MovieWithPricePln
    {
        public Movie Movie { get; set; } = null!;
        public decimal PriceUSD { get; set; }
        public decimal PricePLN { get; set; }
        public decimal UsdRate { get; set; }
        public string RateDate { get; set; } = string.Empty;
    }
}