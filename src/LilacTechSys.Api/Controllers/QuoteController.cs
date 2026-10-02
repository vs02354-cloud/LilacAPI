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
    public class QuoteController : ControllerBase
    {
        private readonly IContactQuoteService _service;

        public QuoteController(IContactQuoteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitQuoteRequest request)
        {
            var result = await _service.SubmitQuoteAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParams)
        {
            var result = await _service.GetQuoteRequestsAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateQuoteStatusRequest request)
        {
            var result = await _service.UpdateQuoteStatusAsync(id, request);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
