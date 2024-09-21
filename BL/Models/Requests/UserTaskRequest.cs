using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BL.Models.Requests
{
    public class UserTaskRequest
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Priority? Priority { get; set; }

        [Required]
        public Status? Status { get; set; }
    }
}
