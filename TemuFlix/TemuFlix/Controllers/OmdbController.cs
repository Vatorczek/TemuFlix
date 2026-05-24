using Microsoft.AspNetCore.Mvc;
using TemuFlix.Models;
using TemuFlix.Services;

namespace TemuFlix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OmdbController : ControllerBase
    {
        private readonly OmdbService _omdb;

        public OmdbController(OmdbService omdb)
        {
            _omdb = omdb;
        }

        // GET /api/omdb/search?title=Inception
        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest(ApiResponse<string>.Fail("Podaj tytuł filmu"));

            var result = await _omdb.SearchByTitleAsync(title);

            if (result == null)
                return NotFound(ApiResponse<string>.Fail($"Nie znaleziono filmu: {title}"));

            return Ok(ApiResponse<OmdbMovie>.Ok(result, "Znaleziono film w OMDB"));
        }

        // GET /api/omdb/query?q=batman
        [HttpGet("query")]
        public async Task<IActionResult> SearchByQuery([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(ApiResponse<string>.Fail("Podaj frazę do wyszukania"));

            var results = await _omdb.SearchByQueryAsync(q);

            return Ok(ApiResponse<List<OmdbMovie>>.Ok(results,
                $"Znaleziono {results.Count} wyników"));
        }

        // POST /api/omdb/import?title=Inception
        // Pobiera z OMDB i zapisuje do bazy
        [HttpPost("import")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> ImportMovie(
            [FromQuery] string title,
            [FromServices] TemuFlix.Data.AppDbContext db)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest(ApiResponse<string>.Fail("Podaj tytuł"));

            var omdb = await _omdb.SearchByTitleAsync(title);
            if (omdb == null)
                return NotFound(ApiResponse<string>.Fail($"Nie znaleziono: {title}"));

            var movie = new Movie
            {
                Title = omdb.Title,
                Year = int.TryParse(omdb.Year, out var y) ? y : 0,
                Director = omdb.Director,
                Genre = omdb.Genre,
                Description = omdb.Plot,
                Rating = double.TryParse(omdb.ImdbRating,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var r) ? r : 0,
                PosterUrl = omdb.Poster != "N/A" ? omdb.Poster : null,
                PriceUSD = 9.99m
            };

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            return Ok(ApiResponse<Movie>.Ok(movie, $"Film '{movie.Title}' zaimportowany z OMDB"));
        }
    }
}