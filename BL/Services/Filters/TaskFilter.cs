using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.Entities.MyTask;

namespace BL.Services.Filters
{
    public class TaskFilter
    {
        public int? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }
    }
}
