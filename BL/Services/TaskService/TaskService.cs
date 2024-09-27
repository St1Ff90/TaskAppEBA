using BL.Mappers;
using BL.Models.DTO;
using BL.Models.Filters;
using BL.Models.Requests;
using DAL.Entities;
using DAL.Repositories.TaskRepository.TaskRepository;
using LinqKit;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace BL.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId, TaskFilter filter)
        {
            try
            {
                _logger.LogInformation("Retrieving tasks for user {UserId} with filter {Filter}.", userId, filter);

                var predicate = CreatePredicate(userId, filter);
                var selectedTasks = await _taskRepository
                    .GetUserTasksAsunc(predicate, filter.PageNumber, filter.PageSize, filter.SortBy, filter.isAsc);

                _logger.LogInformation("{TaskCount} tasks retrieved for user {UserId}.", selectedTasks.Count(), userId);

                return selectedTasks.Select(x => TaskMapper.MapModelToTaskDto(x)!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks for user {UserId}.", userId);
                throw new Exception("An error occurred while retrieving tasks.");
            }
        }

        public async Task<TaskDto> CreateUserTaskAsync(Guid userId, UserTaskRequest newUserTaskRequest)
        {
            try
            {
                _logger.LogInformation("Creating a new task for user {UserId}.", userId);

                var task = TaskMapper.MapToTaskModel(newUserTaskRequest);
                task.Id = Guid.NewGuid();
                task.UserId = userId;
                var currentDate = DateTime.UtcNow.Date;
                task.CreatedAt = currentDate;
                task.UpdatedAt = currentDate;

                var entity = await _taskRepository.CreateAsync(task);

                _logger.LogInformation("Task with ID {TaskId} created for user {UserId}.", task.Id, userId);

                return TaskMapper.MapModelToTaskDto(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task for user {UserId}.", userId);
                throw new Exception("An error occurred while creating the task.");
            }
        }

        public async Task<bool> DeleteUserTaskAsync(Guid userId, Guid taskId)
        {
            try
            {
                _logger.LogInformation("Deleting task {TaskId} for user {UserId}.", taskId, userId);

                var result = await _taskRepository.DeleteUserTaskAsync(userId, taskId);

                if (result)
                {
                    _logger.LogInformation("Task {TaskId} successfully deleted for user {UserId}.", taskId, userId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete task {TaskId} for user {UserId}. Task not found.", taskId, userId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {TaskId} for user {UserId}.", taskId, userId);
                throw new Exception("An error occurred while deleting the task.");
            }
        }

        public async Task<TaskDto?> GetUserTaskByIdAsync(Guid userId, Guid taskId)
        {
            try
            {
                _logger.LogInformation("Retrieving task {TaskId} for user {UserId}.", taskId, userId);

                var task = await _taskRepository.GetUserTaskAsync(userId, taskId);

                if (task == null)
                {
                    _logger.LogWarning("Task {TaskId} not found for user {UserId}.", taskId, userId);
                    return null;
                }

                _logger.LogInformation("Task {TaskId} retrieved for user {UserId}.", taskId, userId);

                return TaskMapper.MapModelToTaskDto(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task {TaskId} for user {UserId}.", taskId, userId);
                throw new Exception("An error occurred while retrieving the task.");
            }
        }

        public async Task<TaskDto?> UpdateUserTaskAsync(Guid userId, Guid taskId, UserTaskRequest userTaskRequest)
        {
            try
            {
                _logger.LogInformation("Updating task {TaskId} for user {UserId}.", taskId, userId);

                var task = await _taskRepository.GetUserTaskAsync(userId, taskId);

                if (task != null)
                {
                    task.UpdatedAt = DateTime.UtcNow;
                    task.Title = userTaskRequest.Title;
                    task.Description = userTaskRequest.Description;
                    task.Status = (int)userTaskRequest.Status;
                    task.DueDate = userTaskRequest.DueDate;
                    task.Priority = (int)userTaskRequest.Priority;

                    var updatedTask = await _taskRepository.UpdateUserTaskAsync(task);

                    _logger.LogInformation("Task {TaskId} updated for user {UserId}.", taskId, userId);
                    return TaskMapper.MapModelToTaskDto(updatedTask);
                }

                _logger.LogWarning("Task {TaskId} not found for user {UserId}, unable to update.", taskId, userId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task {TaskId} for user {UserId}.", taskId, userId);
                throw new Exception("An error occurred while updating the task. Please try again later.");
            }
        }

        private Expression<Func<UserTask, bool>> CreatePredicate(Guid userId, TaskFilter filter)
        {
            var predicate = PredicateBuilder.New<UserTask>(t => t.UserId == userId);

            if (filter.Status.HasValue)
            {
                predicate = predicate.And(t => t.Status == filter.Status.Value);
            }
            if (filter.DueDate.HasValue)
            {
                predicate = predicate.And(t => t.DueDate!.Value.Date == filter.DueDate.Value.Date);
            }
            if (filter.Priority.HasValue)
            {
                predicate = predicate.And(t => t.Priority == filter.Priority.Value);
            }

            return predicate;
        }

    }
}
