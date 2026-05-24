using System.Text.Json.Serialization;

namespace TemuFlix.Models
{
    public class OmdbMovie
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("Year")]
        public string Year { get; set; } = string.Empty;

        [JsonPropertyName("Director")]
        public string Director { get; set; } = string.Empty;

        [JsonPropertyName("Genre")]
        public string Genre { get; set; } = string.Empty;

        [JsonPropertyName("Plot")]
        public string Plot { get; set; } = string.Empty;

        [JsonPropertyName("Poster")]
        public string Poster { get; set; } = string.Empty;

        [JsonPropertyName("imdbRating")]
        public string ImdbRating { get; set; } = string.Empty;

        [JsonPropertyName("imdbID")]
        public string ImdbId { get; set; } = string.Empty;

        [JsonPropertyName("Response")]
        public string Response { get; set; } = string.Empty;

        [JsonPropertyName("Error")]
        public string? Error { get; set; }
    }
}