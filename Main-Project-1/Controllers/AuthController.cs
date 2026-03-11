using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MainProject1.DTOs;
using MainProject1.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainProject1.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(SustainabilityDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST /auth/register
        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("An account with this email already exists.");

            var user = new User
            {
                Email        = dto.Email,
                PasswordHash = PasswordHelper.Hash(dto.Password),
                Name         = dto.Name,
                CreatedAt    = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Seed a Home for every new user
            _context.Homes.Add(new Home
            {
                UserId    = user.Id,
                Name      = $"{user.Name}'s Home",
                Address   = "Enter your address",
                CreatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            var token = GenerateToken(user);

            return Ok(new
            {
                token,
                user = new { user.Id, user.Name, user.Email }
            });
        }

        // POST /auth/login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            // Support legacy plain-text (migration path) and hashed passwords
            bool passwordValid = PasswordHelper.IsHashed(user.PasswordHash)
                ? PasswordHelper.Verify(dto.Password, user.PasswordHash)
                : user.PasswordHash == dto.Password; // legacy plain-text fallback

            if (!passwordValid)
                return Unauthorized("Invalid email or password.");

            // Upgrade plain-text password to hash on successful login
            if (!PasswordHelper.IsHashed(user.PasswordHash))
            {
                user.PasswordHash = PasswordHelper.Hash(dto.Password);
                _context.SaveChanges();
            }

            var token = GenerateToken(user);

            return Ok(new
            {
                token,
                user = new { user.Id, user.Name, user.Email }
            });
        }

        // GET /auth/profile/{userId}
        [HttpGet("profile/{userId}")]
        public IActionResult GetProfile(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.Name, user.Email, user.CreatedAt });
        }

        // PUT /auth/profile/{userId}
        [HttpPut("profile/{userId}")]
        public IActionResult UpdateProfile(int userId, UpdateUserDto dto)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            user.Name      = dto.Name;
            user.Email     = dto.Email;
            user.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(new { user.Id, user.Name, user.Email });
        }

        // ---- Private helpers ----

        private string GenerateToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds      = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry     = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiryMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name",                        user.Name),
                new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             jwtSection["Issuer"],
                audience:           jwtSection["Audience"],
                claims:             claims,
                expires:            expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}