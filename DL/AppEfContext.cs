using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppEfContext : DbContext
    {
        public DbSet<User> MyUsers { get; set; }
        public DbSet<UserTask> MyTasks { get; set; }

        public AppEfContext(DbContextOptions<AppEfContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.UserTask>(entity =>
            {
                entity
                    .Property(x => x.Title)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

                //entity.Property(x=>x.Priority).index
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
