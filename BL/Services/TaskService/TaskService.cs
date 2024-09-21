using BL.Mappers;
using BL.Models;
using BL.Models.DTO;
using BL.Models.Filters;
using BL.Models.Requests;
using DAL.Entities;
using DAL.Repositories.TaskRepository.TaskRepository;
using LinqKit;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId, TaskFilter filter)
        {
            var predicat = CreatePredicate(userId, filter);
            var selectedTasks = await _taskRepository
              .GetUserTasksAsunc(predicat, filter.PageNumber, filter.PageSize,
              filter.SortBy,
              filter.isAsc);

            return selectedTasks.Select(x => TaskMapper.MapModelToTaskDto(x)!);
        }

        public async Task<TaskDto> CreateUserTaskAsync(Guid userId, UserTaskRequest newUserTaskRequest)
        {
            var task = TaskMapper.MapToTaskModel(newUserTaskRequest);
            task.Id = Guid.NewGuid();
            task.UserId = userId;
            var currentDate = DateTime.UtcNow.Date;
            task.CreatedAt = currentDate;
            task.UpdatedAt = currentDate;

            var entity = await _taskRepository.CreateAsync(task);
            return TaskMapper.MapModelToTaskDto(entity);
        }

        public async Task DeleteUserTaskAsync(Guid userId, Guid taskId)
        {
            //_logger.LogWarning($"Delete task request from user:{userId}. Task Id: {taskId}")
            await _taskRepository.DeleteUserTaskAsync(userId, taskId);
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

        public async Task<TaskDto?> GetUserTaskByIdAsync(Guid userId, Guid taskId)
        {
            var task = await _taskRepository
              .GetUserTaskAsync(userId, taskId);

            return TaskMapper.MapModelToTaskDto(task);
        }

        public async Task<TaskDto?> UpdateUserTaskAsync(Guid userId, Guid taskId, UserTaskRequest userTaskRequest)
        {
            //var userTask = new UserTask
            //{
            //    Id = taskId,
            //    UserId = userId,
            //    Description = userTaskRequest.Description,
            //    DueDate = userTaskRequest.DueDate,
            //    Priority = (int?)userTaskRequest.Priority,
            //    Title = userTaskRequest.Title,
            //    Status = (int?)userTaskRequest.Status
            //}
            var task = await _taskRepository
              .GetUserTaskAsync(userId, taskId);

            if (task != null)
            {
                task.UpdatedAt = DateTime.UtcNow;
                task.Title = userTaskRequest.Title;
                task.Description = userTaskRequest.Description;
                task.Status = (int)userTaskRequest.Status;
                task.DueDate = userTaskRequest.DueDate;
                task.Priority = (int)userTaskRequest.Priority;

                return Mappers.TaskMapper.MapModelToTaskDto(await _taskRepository.UpdateUserTaskAsync(task));
            }
            return null;
        }
    }
}
