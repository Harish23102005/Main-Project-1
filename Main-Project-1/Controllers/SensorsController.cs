using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainProject1.DTOs;

namespace MainProject1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("sensors")]
    public class SensorsController : ControllerBase
    {
        private readonly SustainabilityDbContext _context;

        public SensorsController(SustainabilityDbContext context)
        {
            _context = context;
        }

        [HttpPost("data")]
        public IActionResult AddSensorData(SensorDataDto dto)
        {
            var sensor = new SensorData
            {
                ApplianceId = dto.ApplianceId,
                ReadingType = dto.ReadingType,
                Value = dto.Value,
                Unit = dto.Unit,
                Timestamp = DateTime.UtcNow
            };

            _context.SensorData.Add(sensor);
            _context.SaveChanges();

            return Ok(sensor);
        }

        [HttpGet("appliance/{applianceId}")]
        public IActionResult GetByAppliance(int applianceId)
        {
            var data = _context.SensorData
                .Where(s => s.ApplianceId == applianceId)
                .ToList();

            return Ok(data);
        }

        [HttpGet("latest/{applianceId}")]
        public IActionResult GetLatest(int applianceId)
        {
            var data = _context.SensorData
                .Where(s => s.ApplianceId == applianceId)
                .OrderByDescending(s => s.Timestamp)
                .FirstOrDefault();

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _context.SensorData.Find(id);

            if (data == null) return NotFound();

            _context.SensorData.Remove(data);
            _context.SaveChanges();

            return Ok();
        }
    }
}