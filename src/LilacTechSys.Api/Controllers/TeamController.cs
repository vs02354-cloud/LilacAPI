using System;
using System.Threading.Tasks;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LilacTechSys.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly IContactQuoteService _service;

        public TeamController(IContactQuoteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetTeamMembersAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamMemberRequest request)
        {
            var result = await _service.CreateTeamMemberAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteTeamMemberAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
