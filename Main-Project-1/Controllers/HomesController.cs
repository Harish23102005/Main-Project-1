using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MainProject1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("homes")]
    public class HomesController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public HomesController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Homes.ToList());
        }

        public class CreateHomeDto
        {
            public string Name { get; set; } = "";
            public string Address { get; set; } = "";
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateHomeDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("userId")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User identity not found.");

            var home = new Home
            {
                Name = dto.Name,
                Address = dto.Address,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Homes.Add(home);
            _context.SaveChanges();
            return Ok(home);
        }
    }
}
