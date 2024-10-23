using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Features.Users.Queries;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : BaseController
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var users = await Mediator.Send(query);
            return Ok(users);
        }
    }
}

