using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TMS.Shared.Model;
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

            modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.TaskObj)
            .WithMany(t => t.UserTasks)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.UserObj)
                .WithMany() 
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<SetupUser> SetupUsers { get; set; }
        public DbSet<SetupTask> SetupTasks { get; set; }
        public DbSet<SetupTag> SetupTags { get; set; }
        public DbSet<SetupStatus> SetupStatuses { get; set; }
        public DbSet<SetupProject> SetupProjects { get; set; }
        public DbSet<SetupDepartment> SetupDepartments { get; set; }
        public DbSet<SetupDesignation> SetupDesignations { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<UserActivityLog> UserActivityLogs { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }



        // 1. Single Object ya Summary ke liye (Error-Free Fix)
        public async Task<T> QueryFirstOrDefaultAsync<T>(string procedureName, Dictionary<string, object> parameters = null) where T : class, new()
        {
            var (query, sqlParams) = BuildProcedureCommand(procedureName, parameters);

            // Pehle data ko list mein fetch karein taaki non-composable SQL ka error na aaye
            var list = await Database
                .SqlQueryRaw<T>(query, sqlParams)
                .ToListAsync();

            return list.FirstOrDefault() ?? new T();
        }

        // 2. Agar procedure se poori List aani ho
        public async Task<List<T>> QueryListAsync<T>(string procedureName, Dictionary<string, object> parameters = null) where T : class
        {
            var (query, sqlParams) = BuildProcedureCommand(procedureName, parameters);

            var result = await Database
                .SqlQueryRaw<T>(query, sqlParams)
                .ToListAsync();

            return result;
        }

        // Helper method jo query aur parameters ko automatically build karega
        private (string query, SqlParameter[] sqlParams) BuildProcedureCommand(string procedureName, Dictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();
            var parameterNames = new List<string>();

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    string paramName = param.Key.StartsWith("@") ? param.Key : "@" + param.Key;
                    sqlParameters.Add(new SqlParameter(paramName, param.Value ?? DBNull.Value));
                    parameterNames.Add(paramName);
                }
            }

            string query = $"EXEC {procedureName}";
            if (parameterNames.Any())
            {
                query += " " + string.Join(", ", parameterNames);
            }

            return (query, sqlParameters.ToArray());
        }

    }



}
