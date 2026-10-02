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
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = await _serviceService.GetAllServicesAsync(includeInactive);
            return Ok(result);
        }

        [HttpGet("{slugOrId}")]
        public async Task<IActionResult> GetBySlugOrId(string slugOrId)
        {
            var result = await _serviceService.GetServiceBySlugOrIdAsync(slugOrId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
        {
            var result = await _serviceService.CreateServiceAsync(request);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetBySlugOrId), new { slugOrId = result.Data!.Slug }, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceRequest request)
        {
            var result = await _serviceService.UpdateServiceAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _serviceService.DeleteServiceAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
