using Microsoft.AspNetCore.Mvc;
using TemuFlix.Data;
using TemuFlix.Models;
using TemuFlix.Services;

namespace TemuFlix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(ApiResponse<string>.Fail("Wszystkie pola są wymagane"));

            if (req.Password.Length < 6)
                return BadRequest(ApiResponse<string>.Fail("Hasło musi mieć minimum 6 znaków"));

            if (_context.Users.Any(u => u.Email == req.Email))
                return BadRequest(ApiResponse<string>.Fail("Email jest już zajęty"));

            if (_context.Users.Any(u => u.Username == req.Username))
                return BadRequest(ApiResponse<string>.Fail("Nazwa użytkownika jest już zajęta"));

            var user = new User
            {
                Username = req.Username,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return Ok(ApiResponse<AuthResponse>.Ok(new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }, "Rejestracja pomyślna"));
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(ApiResponse<string>.Fail("Email i hasło są wymagane"));

            var user = _context.Users.FirstOrDefault(u => u.Email == req.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized(ApiResponse<string>.Fail("Nieprawidłowy email lub hasło"));

            var token = _jwtService.GenerateToken(user);

            return Ok(ApiResponse<AuthResponse>.Ok(new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            }, "Logowanie pomyślne"));
        }

        // GET /api/auth/me  (wymaga tokena)
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var username = User.Identity?.Name;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(ApiResponse<object>.Ok(new { username, email, role }, "Dane zalogowanego użytkownika"));
        }
    }
}