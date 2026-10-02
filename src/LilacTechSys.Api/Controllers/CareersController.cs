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
    public class CareersController : ControllerBase
    {
        private readonly ICareerService _careerService;
        private readonly IFileStorageService _fileStorage;

        public CareersController(ICareerService careerService, IFileStorageService fileStorage)
        {
            _careerService = careerService;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveJobs()
        {
            var result = await _careerService.GetActiveJobOpeningsAsync();
            return Ok(result);
        }

        [HttpGet("{slugOrId}")]
        public async Task<IActionResult> GetBySlugOrId(string slugOrId)
        {
            var result = await _careerService.GetJobOpeningBySlugOrIdAsync(slugOrId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("{id:guid}/apply")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Apply(Guid id, [FromForm] SubmitApplicationRequest request)
        {
            var result = await _careerService.SubmitApplicationAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobOpeningRequest request)
        {
            var result = await _careerService.CreateJobOpeningAsync(request);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetBySlugOrId), new { slugOrId = result.Data!.Slug }, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateJobOpeningRequest request)
        {
            var result = await _careerService.UpdateJobOpeningAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _careerService.DeleteJobOpeningAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("applications")]
        public async Task<IActionResult> GetApplications([FromQuery] QueryParameters queryParams)
        {
            var result = await _careerService.GetApplicationsAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("applications/{id:guid}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(Guid id, [FromBody] UpdateApplicationStatusRequest request)
        {
            var result = await _careerService.UpdateApplicationStatusAsync(id, request);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("applications/resume")]
        public async Task<IActionResult> DownloadResume([FromQuery] string path)
        {
            var file = await _fileStorage.GetResumeFileAsync(path);
            if (file == null) return NotFound("Resume file not found on server.");
            return File(file.Value.Bytes, file.Value.ContentType, file.Value.FileName);
        }
    }
}
