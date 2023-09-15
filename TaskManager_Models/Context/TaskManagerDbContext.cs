using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Emit;
using TaskManager_Models.Entities.Domains.Auth;
using TaskManager_Models.Entities.Domains.Notifications;
using TaskManager_Models.Entities.Domains.Projects;
using TaskManager_Models.Entities.Domains.Tasks;
using TaskManager_Models.Entities.Domains.User;


namespace TaskManager_Models.Context
{
    public class TaskManagerDbContext : IdentityDbContext<ApplicationUser>
    {

        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {
        }


        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TaskTodo> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {

            foreach (IMutableForeignKey relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            //builder.Entity<UserTaskAssignment>()
            //    .HasKey(uta => new { uta.UserId, uta.TaskId });

            //builder.Entity<UserTaskAssignment>()
            //    .HasOne(uta => uta.User)
            //    .WithMany(u => u.AssignedTasks)
            //    .HasForeignKey(uta => uta.UserId);

            //builder.Entity<UserTaskAssignment>()
            //    .HasOne(uta => uta.Task)
            //    .WithMany(t => t.AssignedUsers)
            //    .HasForeignKey(uta => uta.TaskId);


            //builder.Entity<ApplicationUser>()
            //   .HasMany(u => u.Projects)
            //   .WithOne(p => p.User)
            //   .HasForeignKey(p => p.UserId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Project>()
            //    .HasMany(p => p.Tasks)
            //    .WithOne(t => t.Project)
            //    .HasForeignKey(t => t.ProjectId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Notification>()
            //   .HasOne(n => n.User)
            //   .WithMany(u => u.Notifications)
            //   .HasForeignKey(n => n.UserId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<ApplicationUser>()
            //  .HasMany(u => u.Notifications)
            //  .WithOne(p => p.User)
            //  .HasForeignKey(p => p.UserId)
            //  .OnDelete(DeleteBehavior.Restrict);




            base.OnModelCreating(builder);
        }


    }

}
