using System;
using System.Threading.Tasks;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LilacTechSys.Api.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _dashboardService.GetDashboardStatsAsync();
            return Ok(result);
        }

        [HttpPatch("submissions/{type}/{id:guid}/stage")]
        public async Task<IActionResult> UpdateStage(string type, Guid id, [FromBody] UpdateSubmissionStageRequest request)
        {
            var result = await _dashboardService.UpdateSubmissionStageAsync(type, id, request.Stage);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
