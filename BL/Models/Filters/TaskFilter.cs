using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace BL.Models.Filters
{
    public class TaskFilter
    {
        [Required]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = Constants.PageSize;

        public int? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }

        public SortField SortBy { get; set; }
        public bool isAsc { get; set; } = true;
    }
}
