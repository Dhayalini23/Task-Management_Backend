using Microsoft.EntityFrameworkCore;
using Task_Management.Models;

namespace Task_Management.Database
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        //public DbSet<Login> Logins { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne(b => b.User)
                .HasForeignKey<Address>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(o => o.Tasks)
                .WithOne(p => p.User)
                .HasForeignKey(o => o.AssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasMany(a => a.CheckLists)
                .WithOne(b => b.TaskItem)
                .HasForeignKey(c => c.TaskId);


            base.OnModelCreating(modelBuilder);
        }
    }


}
