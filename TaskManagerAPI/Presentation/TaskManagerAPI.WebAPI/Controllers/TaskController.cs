using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Application.Dtos.UpdateTask;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem;
using TaskManagerAPI.Application.TasksManage.TaskItems.Queries;
using TaskManagerAPI.Application.TasksManage.TaskLists.Commands;
using TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskListForUser;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class TaskController : BaseController
    {
        [HttpPost("taskItems")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CreateTaskItem([FromBody] CreateTaskItemDto createTaskItemDto)
        {
            var taskItemId = await Mediator.Send(new CreateTaskItemCommand
            {
                CreateTaskItemDto = createTaskItemDto
            });

            return Created(string.Empty, taskItemId);
        }

        [HttpPost("taskLists")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateTaskList([FromBody] CreateTaskListDto createTaskListDto)
        {
            var taskListId = await Mediator.Send(new CreateTaskListCommand
            {
                CreateTaskListDto = createTaskListDto
            });

            return Created(string.Empty, taskListId);
        }

        [HttpGet("taskLists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskListForUserVm>> GetTaskListForUser()
        {
            var query = new GetTaskListForUserQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("taskItems/{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskItemForUserByIdVm>> GetTaskItems([FromRoute] int taskItemId)
        {
            var query = new GetTaskItemForUserByIdQuery
            {
                TaskItemId = taskItemId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("taskItems/{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateTaskItem([FromRoute] int taskItemId, [FromBody] UpdateTaskItemDto updateTaskItemDto)
        {
            var query = new UpdateTaskItemCommand
            {
                UpdateTaskItemDto = updateTaskItemDto
            };

            await Mediator.Send(query);
            return NoContent();
        }
    }
}
