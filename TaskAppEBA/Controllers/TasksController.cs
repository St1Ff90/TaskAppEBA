using BL.Models.DTO;
using BL.Models.Filters;
using BL.Models.Requests;
using BL.Services.TaskService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskAppEBA.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TasksController(ITaskService _taskService, ILogger<TasksController> _logger) : ControllerBase
    {
        private readonly Func<Guid> GetUserIdFromClaims;

        /// <summary>
        /// Retrieves a user task by the specified task ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the user task if found, along with a <see cref="System.Net.HttpStatusCode.OK"/> status; 
        /// otherwise, returns a <see cref="System.Net.HttpStatusCode.NotFound"/> status.
        /// </returns>
        [HttpGet("{id}", Name = nameof(GetTaskByIdAsync))]
        public async Task<ActionResult<TaskDto?>> GetTaskByIdAsync(Guid id)
        {
            var userId = GetUserIdFromClaims();
            _logger.LogInformation("User {UserId} is retrieving task with ID {TaskId}.", userId, id);

            var task = await _taskService.GetUserTaskByIdAsync(userId, id);

            if (task != null)
            {
                _logger.LogInformation("Task with ID {TaskId} found.", id);
                return Ok(task);
            }

            _logger.LogWarning("Task with ID {TaskId} not found.", id);
            return NotFound(new { Message = $"Task with ID {id} not found.", TaskId = id });
        }

        /// <summary>
        /// Retrieves user tasks based on the provided <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The filter criteria for tasks.</param>
        /// <returns>
        /// A list of tasks with a <see cref="System.Net.HttpStatusCode.OK"/> status if tasks are found; 
        /// otherwise, returns a <see cref="System.Net.HttpStatusCode.NoContent"/> status if no tasks are found.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksAsync([FromQuery] TaskFilter filter)
        {
            var userId = GetUserIdFromClaims();
            _logger.LogInformation("User {UserId} is retrieving tasks with filter {@Filter}.", userId, filter);

            var tasks = await _taskService.GetUserTasksAsync(userId, filter);

            if (tasks == null || !tasks.Any())
            {
                _logger.LogInformation("No tasks found for user {UserId}.", userId);
                return NoContent();
            }

            _logger.LogInformation("{TaskCount} tasks found for user {UserId}.", tasks.Count(), userId);
            return Ok(tasks);
        }

        /// <summary>
        /// Creates a new user task based on the provided <paramref name="newUserTaskRequest"/>.
        /// If the task is successfully created, returns a <see cref="System.Net.HttpStatusCode.Created"/> response with the location of the new task.
        /// If an error occurs during the creation process, returns a <see cref="System.Net.HttpStatusCode.InternalServerError"/> response with an error message.
        /// </summary>
        /// <param name="newUserTaskRequest">The request object containing the details of the task to be created.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing an <see cref="ActionResult"/> with the created <see cref="TaskDto"/>.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateUserTask(UserTaskRequest newUserTaskRequest)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                _logger.LogInformation("User {UserId} is creating a new task.", userId);

                var createdTask = await _taskService.CreateUserTaskAsync(userId, newUserTaskRequest);
                _logger.LogInformation("Task with ID {TaskId} created successfully.", createdTask.Id);

                return CreatedAtAction(nameof(GetTaskByIdAsync), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the task.");
                return StatusCode(500, "An error occurred while creating the task." + ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing user task identified by the specified <paramref name="id"/>.
        /// Returns a <see cref="System.Net.HttpStatusCode.BadRequest"/> if the request body is null, 
        /// a <see cref="System.Net.HttpStatusCode.NotFound"/> if the task does not exist, 
        /// or a <see cref="System.Net.HttpStatusCode.NoContent"/> if the task is successfully updated.
        /// </summary>
        /// <param name="id">The ID of the task to be updated.</param>
        /// <param name="userTaskRequest">The request object containing the updated details of the task.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing an <see cref="ActionResult"/> with the updated <see cref="TaskDto"/>, 
        /// or appropriate error responses.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto?>> UpdateTask(Guid id, [FromBody] UserTaskRequest userTaskRequest)
        {
            if (userTaskRequest == null)
            {
                _logger.LogWarning("Update request body cannot be null for task ID {TaskId}.", id);
                return BadRequest(new { Message = "Request body cannot be null." });
            }

            var userId = GetUserIdFromClaims();
            _logger.LogInformation("User {UserId} is updating task with ID {TaskId}.", userId, id);

            var result = await _taskService.UpdateUserTaskAsync(userId, id, userTaskRequest);

            if (result == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for user {UserId}.", id, userId);
                return NotFound(new { Message = $"Task with ID {id} not found." });
            }

            _logger.LogInformation("Task with ID {TaskId} updated successfully.", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user task identified by the specified <paramref name="taskId"/>.
        /// Returns a <see cref="System.Net.HttpStatusCode.NoContent"/> if the task is successfully deleted,
        /// or a <see cref="System.Net.HttpStatusCode.NotFound"/> if the task does not exist.
        /// </summary>
        /// <param name="taskId">The ID of the task to be deleted.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing an <see cref="ActionResult"/> indicating the result of the operation.
        /// </returns>

        [HttpDelete("{taskId}")]
        public async Task<ActionResult<bool>> DeleteUserTask(Guid taskId)
        {
            var userId = GetUserIdFromClaims();
            _logger.LogInformation("User {UserId} is attempting to delete task with ID {TaskId}.", userId, taskId);

            var isDeleted = await _taskService.DeleteUserTaskAsync(userId, taskId);

            if (!isDeleted)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for user {UserId}.", taskId, userId);
                return NotFound(new { Message = $"Task with ID {taskId} not found." });
            }

            _logger.LogInformation("Task with ID {TaskId} deleted successfully.", taskId);
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
