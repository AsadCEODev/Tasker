using TMS.API.Services;
using TMS.API.Services.SetupServices;
using TSM.API.Services;
using TSM.API.Services.SetupServices;

namespace TMS.API.Services
{
    public static class RegisterServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISetupUserService, SetupUserService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ISetupProjectService, SetupProjectService>();
            services.AddScoped<IAssignProjectService, AssignProjectService>();
            services.AddScoped<IUserAssignedTasksService, UserAssignedTasksService>();
            services.AddScoped<ISetupDepartmentService, SetupDepartmentService>();
            services.AddScoped<ISetupDesignationService, SetupDesignationService>();
            services.AddScoped<IAppScreenService, AppScreenService>();
            services.AddScoped<IAppRolesService, AppRolesService>();
            services.AddScoped<IAppUserRoleService, AppUserRoleService>();

            
            return services;
        }
    }
}
