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
    public class ContactController : ControllerBase
    {
        private readonly IContactQuoteService _service;

        public ContactController(IContactQuoteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitContactRequest request)
        {
            var result = await _service.SubmitContactAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParams)
        {
            var result = await _service.GetContactMessagesAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("{id:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var result = await _service.MarkContactAsReadAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
