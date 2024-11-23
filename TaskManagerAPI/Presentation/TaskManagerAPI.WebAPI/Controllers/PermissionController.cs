using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.CreatePermission;
using TaskManagerAPI.Application.PermissionsManage.Commands;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/permissions")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class PermissionController : BaseController
    {
        [HttpPost("users/{userId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePermission([FromRoute] string userId, [FromBody] CreatePermissionDto createPermissionDto)
        {
            var command = new CreatePermissionCommand
            {
                UserId = userId,
                CreatePermissionDto = createPermissionDto
            };

            var permissionId = await Mediator.Send(command);
            return Created(string.Empty, permissionId);
        }
    }
}
