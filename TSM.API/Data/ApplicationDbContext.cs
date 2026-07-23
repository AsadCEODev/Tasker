using Microsoft.EntityFrameworkCore;
using TMS.Shared.Model.Setup;

namespace TMS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SetupUser> SetupUsers { get; set; }
        public DbSet<SetupTask> SetupTasks { get; set; }
        public DbSet<SetupTag> SetupTags { get; set; }
        public DbSet<SetupStatus> SetupStatuses { get; set; }

    }

}
