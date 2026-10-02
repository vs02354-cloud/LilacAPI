using System;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LilacTechSys.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] QueryParameters queryParams)
        {
            var result = await _projectService.GetProjectsAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            var result = await _projectService.GetFeaturedProjectsAsync();
            return Ok(result);
        }

        [HttpGet("{slugOrId}")]
        public async Task<IActionResult> GetBySlugOrId(string slugOrId)
        {
            var result = await _projectService.GetProjectBySlugOrIdAsync(slugOrId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var result = await _projectService.CreateProjectAsync(request);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetBySlugOrId), new { slugOrId = result.Data!.Slug }, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
        {
            var result = await _projectService.UpdateProjectAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
