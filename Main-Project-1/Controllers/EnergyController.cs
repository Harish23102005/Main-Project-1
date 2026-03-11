using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("energy")]
    public class EnergyController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public EnergyController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpPost("record")]
        public IActionResult RecordEnergy(CreateEnergyUsageDto dto)
        {
            var energy = new EnergyUsage
            {
                ApplianceId = dto.ApplianceId,
                KwhConsumed = dto.KwhConsumed,
                PeakUsage = dto.PeakUsage,
                CostEstimate = dto.CostEstimate,
                Date = DateTime.UtcNow
            };

            _context.EnergyUsages.Add(energy);
            _context.SaveChanges();

            return Ok(energy);
        }

        [HttpGet("appliance/{applianceId}")]
        public IActionResult GetByAppliance(int applianceId)
        {
            return Ok(_context.EnergyUsages
                .Where(e => e.ApplianceId == applianceId)
                .ToList());
        }

        [HttpGet("home/{homeId}")]
        public IActionResult GetHomeEnergy(int homeId)
        {
            return Ok(_context.EnergyUsages.ToList());
        }

        [HttpGet("report/{homeId}")]
        public IActionResult EnergyReport(int homeId)
        {
            var total = _context.EnergyUsages.Sum(e => e.KwhConsumed);

            return Ok(new { TotalEnergy = total });
        }
    }
}