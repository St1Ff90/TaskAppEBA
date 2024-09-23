using BL.Models.DTO;
using BL.Models.Filters;
using BL.Models.Requests;

namespace BL.Services.TaskService
{
    public interface ITaskService
    {
        Task<TaskDto> CreateUserTaskAsync(Guid userId, UserTaskRequest userTaskRequest);
        Task<bool> DeleteUserTaskAsync(Guid userId, Guid taskId);
        Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId, TaskFilter filter);
        Task<TaskDto?> GetUserTaskByIdAsync(Guid userId, Guid taskId);
        Task<TaskDto?> UpdateUserTaskAsync(Guid userId, Guid taskId, UserTaskRequest userTaskRequest);
    }
}