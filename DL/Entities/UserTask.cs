namespace DAL.Entities
{
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
