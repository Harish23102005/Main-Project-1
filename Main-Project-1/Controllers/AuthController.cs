using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public AuthController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.Password,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email && u.PasswordHash == dto.Password);

            if (user == null) return Unauthorized();

            return Ok(user);
        }

        [HttpGet("profile/{userId}")]
        public IActionResult GetProfile(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPut("profile/{userId}")]
        public IActionResult UpdateProfile(int userId, UpdateUserDto dto)
        {
            var user = _context.Users.Find(userId);

            if (user == null) return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(user);
        }
    }
}