using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class MyTask : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public enum Status { Panding, In_Progress, Completed }
        public enum Priority { Low, Medium, High }

        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
