using Microsoft.AspNetCore.Mvc;
using PhotogramAPI.Services;

namespace PhotogramAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IUserService _userService;

        public SearchController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<ActionResult> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var result = await _userService.SearchUsers(query);

            return Ok(result);
        }
    }
}
