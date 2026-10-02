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
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPosts([FromQuery] QueryParameters queryParams)
        {
            var result = await _blogService.GetBlogPostsAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 3)
        {
            var result = await _blogService.GetRecentBlogPostsAsync(count);
            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _blogService.GetCategoriesAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var result = await _blogService.CreateCategoryAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{slugOrId}")]
        public async Task<IActionResult> GetBySlugOrId(string slugOrId)
        {
            var result = await _blogService.GetBlogPostBySlugOrIdAsync(slugOrId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogRequest request)
        {
            var result = await _blogService.CreateBlogPostAsync(request);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetBySlugOrId), new { slugOrId = result.Data!.Slug }, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlogRequest request)
        {
            var result = await _blogService.UpdateBlogPostAsync(id, request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _blogService.DeleteBlogPostAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
