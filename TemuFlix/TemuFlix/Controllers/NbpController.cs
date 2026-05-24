using Microsoft.AspNetCore.Mvc;
using TemuFlix.Data;
using TemuFlix.Models;
using TemuFlix.Services;
using Microsoft.EntityFrameworkCore;

namespace TemuFlix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NbpController : ControllerBase
    {
        private readonly NbpService _nbp;
        private readonly AppDbContext _context;

        public NbpController(NbpService nbp, AppDbContext context)
        {
            _nbp = nbp;
            _context = context;
        }

        // GET /api/nbp/usd
        [HttpGet("usd")]
        public async Task<IActionResult> GetUsdRate()
        {
            var rate = await _nbp.GetUsdRateAsync();
            if (rate == null)
                return StatusCode(503, ApiResponse<string>.Fail("NBP API niedostępne"));

            return Ok(ApiResponse<NbpRate>.Ok(rate, $"Kurs USD: {rate.Mid} PLN"));
        }

        // GET /api/nbp/rate/{currency}  np. /api/nbp/rate/EUR
        [HttpGet("rate/{currency}")]
        public async Task<IActionResult> GetRate(string currency)
        {
            var rate = await _nbp.GetRateAsync(currency);
            if (rate == null)
                return NotFound(ApiResponse<string>.Fail(
                    $"Nie znaleziono kursu dla: {currency.ToUpper()}"));

            return Ok(ApiResponse<NbpRate>.Ok(rate,
                $"Kurs {currency.ToUpper()}: {rate.Mid} PLN"));
        }

        // GET /api/nbp/movies/prices
        // Zwraca wszystkie filmy z cenami w PLN
        [HttpGet("movies/prices")]
        public async Task<IActionResult> GetMoviesWithPln()
        {
            var rate = await _nbp.GetUsdRateAsync();
            if (rate == null)
                return StatusCode(503, ApiResponse<string>.Fail("NBP API niedostępne"));

            var movies = await _context.Movies.ToListAsync();

            var result = movies.Select(m => new MovieWithPricePln
            {
                Movie = m,
                PriceUSD = m.PriceUSD,
                PricePLN = Math.Round(m.PriceUSD * rate.Mid, 2),
                UsdRate = rate.Mid,
                RateDate = rate.EffectiveDate
            }).ToList();

            return Ok(ApiResponse<List<MovieWithPricePln>>.Ok(result,
                $"Kurs USD/PLN z dnia {rate.EffectiveDate}: {rate.Mid}"));
        }
    }
}