using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemuFlix.Data;
using TemuFlix.Models;

namespace TemuFlix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/movies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(ApiResponse<List<Movie>>.Ok(movies, $"Znaleziono {movies.Count} filmów"));
        }

        // GET /api/movies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound(ApiResponse<Movie>.Fail($"Film o id={id} nie istnieje"));

            return Ok(ApiResponse<Movie>.Ok(movie));
        }

        // POST /api/movies
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Movie movie)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
                return BadRequest(ApiResponse<Movie>.Fail("Tytuł jest wymagany"));

            if (movie.Year < 1888 || movie.Year > DateTime.Now.Year + 5)
                return BadRequest(ApiResponse<Movie>.Fail("Nieprawidłowy rok"));

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = movie.Id },
                ApiResponse<Movie>.Ok(movie, "Film dodany pomyślnie"));
        }

        // PUT /api/movies/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Movie updated)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound(ApiResponse<Movie>.Fail($"Film o id={id} nie istnieje"));

            movie.Title = updated.Title;
            movie.Year = updated.Year;
            movie.Director = updated.Director;
            movie.Description = updated.Description;
            movie.Genre = updated.Genre;
            movie.Rating = updated.Rating;
            movie.PriceUSD = updated.PriceUSD;
            movie.PosterUrl = updated.PosterUrl;

            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Movie>.Ok(movie, "Film zaktualizowany"));
        }

        // DELETE /api/movies/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound(ApiResponse<Movie>.Fail($"Film o id={id} nie istnieje"));

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<Movie>.Fail($"Film '{movie.Title}' usunięty"));
        }
    }
}