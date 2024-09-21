namespace DAL.Entities
{
    public enum Status { Pending = 1, In_Progress = 2, Completed = 3 }
    public enum Priority { Low = 1, Medium = 2, High = 3 }

    public class UserTask : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }

        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
