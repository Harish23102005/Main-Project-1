using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [Authorize]
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
            // Validate FK references exist to prevent DbUpdateException
            if (!_context.Homes.Any(h => h.Id == dto.HomeId))
                return BadRequest($"Home with Id={dto.HomeId} does not exist.");

            if (!_context.ApplianceTypes.Any(t => t.Id == dto.TypeId))
                return BadRequest($"ApplianceType with Id={dto.TypeId} does not exist.");

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

            return Ok(new
            {
                appliance.Id,
                appliance.HomeId,
                applianceTypeId = appliance.ApplianceTypeId,
                appliance.DeviceIdentifier,
                appliance.Name,
                appliance.Model,
                appliance.Status,
                appliance.InstalledAt
            });
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

        public class UpdateStatusDto
        {
            public string Status { get; set; } = "";
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var allowed = new[] { "Active", "Inactive", "Maintenance" };
            if (!allowed.Contains(dto.Status))
                return BadRequest("Invalid status. Allowed: Active, Inactive, Maintenance.");

            var appliance = _context.Appliances.Find(id);
            if (appliance == null) return NotFound();

            appliance.Status = dto.Status;
            _context.SaveChanges();

            return Ok(appliance);
        }
    }
}