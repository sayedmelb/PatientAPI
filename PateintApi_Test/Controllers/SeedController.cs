using Microsoft.AspNetCore.Mvc;
using PatientAPI.Infrastructure.Services;

namespace PatientAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IDataSeedingService _seedingService;

        public SeedController(IDataSeedingService seedingService)
        {
            _seedingService = seedingService;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            var result = await _seedingService.SeedDatabaseAsync();
            return result.IsSuccess ? Ok("Database seeded successfully") : BadRequest(result.Error);
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearDatabase()
        {
            var result = await _seedingService.ClearDatabaseAsync();
            return result.IsSuccess ? Ok("Database cleared successfully") : BadRequest(result.Error);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetSeedingStatus()
        {
            var isSeeded = await _seedingService.IsDatabaseSeededAsync();
            return Ok(new { IsSeeded = isSeeded });
        }
    }
}
