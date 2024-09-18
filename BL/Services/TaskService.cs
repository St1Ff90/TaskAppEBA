using BL.DTO;
using BL.Mappers;
using BL.Services.Filters;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskDto> GetTasks(Guid userId, TaskFilter filter)
        {
            var selectedTasks = _taskRepository.GetUserTasks(userId, filter.Status, filter.DueDate, filter.Priority);

            foreach (var task in selectedTasks)
            {
                yield return TaskMapper.MapModelToTaskDto(task);
            }
        }
    }
}
