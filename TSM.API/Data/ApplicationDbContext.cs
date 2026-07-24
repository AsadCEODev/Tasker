using Microsoft.EntityFrameworkCore;
using TMS.Shared.Model.Setup;

namespace TMS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<SetupTask>()
                .HasOne(t => t.UserObj)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SetupTask>()
                .HasOne(t => t.StatusObj)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SetupTask>()
                .HasOne(t => t.TagObj)
                .WithMany()
                .HasForeignKey(t => t.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SetupTask>()
                .HasOne(t => t.ProjectObj)
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<SetupUser> SetupUsers { get; set; }
        public DbSet<SetupTask> SetupTasks { get; set; }
        public DbSet<SetupTag> SetupTags { get; set; }
        public DbSet<SetupStatus> SetupStatuses { get; set; }
        public DbSet<SetupProject> SetupProjects { get; set; }

    }

}
