using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.Entities.MyTask;

namespace BL.Requests
{
    public class NewUserTaskRequest //not used yet
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Priority Priority { get; set; }
    }
}
