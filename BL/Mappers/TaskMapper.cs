using BL.DTO;
using BL.Requests;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto MapModelToTaskDto(MyTask task)
        {
            return new TaskDto()
            {
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = (Priority)task.Priority!,
                Status = (Status)task.Status!,
                Title = task.Title,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}
