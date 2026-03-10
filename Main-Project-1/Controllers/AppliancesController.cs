using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [ApiController]
    [Route("appliances")]
    public class AppliancesController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public AppliancesController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Appliances.ToList());
        }

        [HttpPost]
        public IActionResult Create(CreateApplianceDto dto)
        {
            var appliance = new Appliance
            {
                HomeId = dto.HomeId,
                ApplianceTypeId = dto.TypeId,
                DeviceIdentifier = dto.DeviceIdentifier,
                Name = dto.Name,
                Model = dto.Model,
                Status = dto.Status,
                InstalledAt = DateTime.UtcNow
            };

            _context.Appliances.Add(appliance);
            _context.SaveChanges();

            return Ok(appliance);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var appliance = _context.Appliances.Find(id);

            if (appliance == null) return NotFound();

            return Ok(appliance);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var appliance = _context.Appliances.Find(id);

            if (appliance == null) return NotFound();

            _context.Appliances.Remove(appliance);
            _context.SaveChanges();

            return Ok();
        }
    }
}