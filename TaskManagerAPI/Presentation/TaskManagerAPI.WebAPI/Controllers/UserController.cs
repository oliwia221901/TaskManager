using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.UsersManage.Users;
using TaskManagerAPI.Application.UsersManage.Users.Queries.GetAllUsers;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UsersVm>> GetUsers()
        {
            var query = new GetUsersQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AllUsersVm>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}

