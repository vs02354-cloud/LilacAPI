using System.Threading.Tasks;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LilacTechSys.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly IContactQuoteService _service;

        public NewsletterController(IContactQuoteService service)
        {
            _service = service;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
        {
            var result = await _service.SubscribeNewsletterAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("subscribers")]
        public async Task<IActionResult> GetSubscribers()
        {
            var result = await _service.GetSubscribersAsync();
            return Ok(result);
        }
    }
}
