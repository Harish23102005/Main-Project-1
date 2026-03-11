using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainProject1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("appliancetypes")]
    public class ApplianceTypesController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public ApplianceTypesController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.ApplianceTypes.ToList());
        }
    }
}
