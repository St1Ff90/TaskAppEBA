using BL.DTO;
using BL.Services;
using BL.Services.Filters;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskAppEBA.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult<IEnumerable<TaskDto>> GetTasks([FromQuery] TaskFilter filter)
        {
           
            Guid userId = Guid.Empty;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdClaim?.Value, out userId);

            var tasks = _taskService.GetTasks(userId, filter);
            return Ok(tasks);


        }
    }
}
