using BL.Models.Filters;
using BL.Models.Requests;
using BL.Services.TaskService;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskAppEBA.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly Func<Guid> GetUserIdFromClaims;
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
            GetUserIdFromClaims = GetUserId;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByIdAsync(Guid id) //Task<IActionResult> vs Task<ActionResult<UserTask>>
        {
            var task = await _taskService.GetUserTaskByIdAsync(GetUserIdFromClaims(), id);

            if (task == null)
            {
                return NotFound($"Taks with ID {id} not found.");
            }

            return Ok(task);
        }


        [HttpGet]
        public async Task<IActionResult> GetTasksAsync([FromQuery] TaskFilter filter)
        {
            var tasks = await _taskService.GetUserTasksAsync(GetUserIdFromClaims(), filter);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserTask(UserTaskRequest newUserTaskRequest)
        {//RequestValidator

            var userId = GetUserId();
            return Ok(await _taskService.CreateUserTaskAsync(GetUserIdFromClaims(), newUserTaskRequest));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUserTask(Guid taskId)
        {
            await _taskService.DeleteUserTaskAsync(GetUserIdFromClaims(), taskId);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UserTaskRequest userTaskRequest)
        {
            if (userTaskRequest == null)
            {
                return BadRequest();
            }

            var result = await _taskService.UpdateUserTaskAsync(GetUserIdFromClaims(), id, userTaskRequest);

            if (result == null)
            {
                return NotFound($"ID {id} NotFound.");
            }

            return NoContent();
        }

        private Guid GetUserId()
        {
            Guid userId = Guid.Empty;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdClaim?.Value, out userId);

            return userId;
        }
    }
}
