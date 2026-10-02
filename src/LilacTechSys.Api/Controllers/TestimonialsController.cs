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
    public class TestimonialsController : ControllerBase
    {
        private readonly IContactQuoteService _service;

        public TestimonialsController(IContactQuoteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool featuredOnly = false)
        {
            var result = await _service.GetTestimonialsAsync(featuredOnly);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestimonialRequest request)
        {
            var result = await _service.CreateTestimonialAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteTestimonialAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
