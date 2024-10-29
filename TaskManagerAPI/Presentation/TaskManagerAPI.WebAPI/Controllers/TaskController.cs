using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Application.TaskItems.Commands;
using TaskManagerAPI.Application.TaskItems.Queries;
using TaskManagerAPI.Application.TaskLists.Commands;
using TaskManagerAPI.Application.TaskLists.Queries.GetTaskListForUser;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : BaseController
    {
        [HttpPost("taskItems")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    }
}

