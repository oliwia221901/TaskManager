using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.PermissionsManage.CreatePermission;
using TaskManagerAPI.Application.Dtos.PermissionsManage.UpdatePermission;
using TaskManagerAPI.Application.PermissionsManage.Commands.CreatePermission;
using TaskManagerAPI.Application.PermissionsManage.Commands.UpdatePermission;

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

            var result = await Mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPut("{permissionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePermission([FromRoute] int permissionId, [FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var command = new UpdatePermissionCommand
            {
                PermissionId = permissionId,
                UpdatePermissionDto = updatePermissionDto
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
