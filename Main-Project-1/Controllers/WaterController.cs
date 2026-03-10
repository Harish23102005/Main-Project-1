using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [ApiController]
    [Route("water")]
    public class WaterController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public WaterController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpPost("record")]
        public IActionResult RecordWater(CreateWaterUsageDto dto)
        {
            var water = new WaterUsage
            {
                ApplianceId = dto.ApplianceId,
                LitersConsumed = dto.LitersConsumed,
                CycleCount = dto.CycleCount,
                CostEstimate = dto.CostEstimate,
                Date = DateTime.UtcNow
            };

            _context.WaterUsages.Add(water);
            _context.SaveChanges();

            return Ok(water);
        }

        [HttpGet("appliance/{applianceId}")]
        public IActionResult GetByAppliance(int applianceId)
        {
            return Ok(_context.WaterUsages
                .Where(w => w.ApplianceId == applianceId)
                .ToList());
        }

        [HttpGet("home/{homeId}")]
        public IActionResult GetHomeWater(int homeId)
        {
            return Ok(_context.WaterUsages.ToList());
        }

        [HttpGet("report/{homeId}")]
        public IActionResult WaterReport(int homeId)
        {
            var total = _context.WaterUsages.Sum(w => w.LitersConsumed);

            return Ok(new { TotalWater = total });
        }
    }
}