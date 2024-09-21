using BL.Models.DTO;
using BL.Models.Requests;
using DAL.Entities;

namespace BL.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto? MapModelToTaskDto(UserTask? task)
        {
            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = (Priority)task.Priority!,
                Status = (Status)task.Status!,
                Title = task.Title,
                UpdatedAt = task.UpdatedAt
            };
        }

        public static UserTask MapToTaskModel(UserTaskRequest request)
        {
            return new UserTask
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = (int)request.Priority,
                Status = (int)request.Status
            };
        }
    }
}
