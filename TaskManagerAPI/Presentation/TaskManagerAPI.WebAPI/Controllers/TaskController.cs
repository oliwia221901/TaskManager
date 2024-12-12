using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.TasksManage.CreateTask;
using TaskManagerAPI.Application.Dtos.TasksManage.UpdateTask;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands.DeleteTaskItem;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem;
using TaskManagerAPI.Application.TasksManage.TaskItems.Queries;
using TaskManagerAPI.Application.TasksManage.TaskLists.Commands;
using TaskManagerAPI.Application.TasksManage.TaskLists.Commands.DeleteTaskList;
using TaskManagerAPI.Application.TasksManage.TaskLists.Commands.UpdateTaskList;
using TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskList;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class TaskController : BaseController
    {
        [HttpPost("taskItems/{taskListId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CreateTaskItem([FromRoute] int taskListId, [FromBody] CreateTaskItemDto createTaskItemDto)
        {
            var taskItemId = await Mediator.Send(new CreateTaskItemCommand
            {
                TaskListId = taskListId,
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
        public async Task<ActionResult<TaskListVm>> GetTaskListForUser()
        {
            var query = new GetTaskListQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("taskItems/{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskItemByIdVm>> GetTaskItems([FromRoute] int taskItemId)
        {
            var query = new GetTaskItemByIdQuery
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
                TaskItemId = taskItemId,
                UpdateTaskItemDto = updateTaskItemDto
            };

            await Mediator.Send(query);
            return NoContent();
        }

        [HttpPut("taskLists/{taskListId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateTaskList([FromRoute] int taskListId, [FromBody] UpdateTaskListDto updateTaskListDto)
        {
            var query = new UpdateTaskListCommand
            {
                TaskListId = taskListId,
                UpdateTaskListDto = updateTaskListDto
            };

            await Mediator.Send(query);
            return NoContent();
        }

        [HttpDelete("taskItems/{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteTaskItem([FromRoute] int taskItemId)
        {
            var query = new DeleteTaskItemCommand
            {
                TaskItemId = taskItemId
            };

            await Mediator.Send(query);
            return NoContent();
        }

        [HttpDelete("taskLists/{taskListId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteTaskList([FromRoute] int taskListId)
        {
            var query = new DeleteTaskListCommand
            {
                TaskListId = taskListId
            };

            await Mediator.Send(query);
            return NoContent();
        }
    }
}
