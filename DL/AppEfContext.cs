using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppEfContext : DbContext
    {
        public DbSet<User> MyUsers { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        public AppEfContext(DbContextOptions<AppEfContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.UserTask>(entity =>
            {
                entity
                    .Property(x => x.Title)
                    .IsRequired();

                entity.HasIndex(x => x.Status);
                entity.HasIndex(x => x.DueDate);
                entity.HasIndex(x => x.Priority);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity
                    .Property(x => x.Username)
                    .IsRequired();

                entity
                    .HasIndex(x => x.Username)
                    .IsUnique();

                entity
                    .HasMany(x => x.Tasks)
                    .WithOne(x => x.User);
            });
        }
    }
}
