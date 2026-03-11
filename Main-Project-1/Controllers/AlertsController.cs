using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public AlertsController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Alerts.ToList());
        }

        [HttpPost]
        public IActionResult Create(CreateAlertDto dto)
        {
            var alert = new Alert
            {
                ApplianceId = dto.ApplianceId,
                AlertType = dto.AlertType,
                Severity = dto.Severity,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false
            };

            _context.Alerts.Add(alert);
            _context.SaveChanges();

            return Ok(alert);
        }

        [HttpPut("{id}/resolve")]
        public IActionResult Resolve(int id)
        {
            var alert = _context.Alerts.Find(id);

            if (alert == null) return NotFound();

            alert.IsResolved = true;

            _context.SaveChanges();

            return Ok(alert);
        }

        [HttpGet("appliance/{applianceId}")]
        public IActionResult GetByAppliance(int applianceId)
        {
            return Ok(_context.Alerts
                .Where(a => a.ApplianceId == applianceId)
                .ToList());
        }
    }
}