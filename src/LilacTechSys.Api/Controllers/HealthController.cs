using System;
using System.Threading.Tasks;
using LilacTechSys.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LilacTechSys.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly LilacDbContext _context;

        public HealthController(LilacDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CheckHealth()
        {
            var dbHealthy = false;
            string? dbError = null;
            try
            {
                dbHealthy = await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                dbHealthy = false;
                dbError = ex.Message + (ex.InnerException != null ? $" -> {ex.InnerException.Message}" : "");
            }

            return Ok(new
            {
                status = dbHealthy ? "Healthy" : "Degraded",
                system = "LilacTechSys Web API",
                version = "1.0.0",
                timestamp = DateTime.UtcNow,
                database = dbHealthy ? "Connected" : "Disconnected",
                databaseError = dbError
            });
        }
    }
}
