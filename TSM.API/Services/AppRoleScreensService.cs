using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model;
using TSM.API.Data;

namespace TSM.API.Services
{
    public class AppRoleScreensService: BaseClassService, IAppRoleScreensService
    {
        private readonly ILogger<AppRoleScreensService> _logger;
        public AppRoleScreensService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<AppRoleScreensService> logger) : base(dbContext, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<List<AppRolesScreenDto>> GetUserPermissionsList()
        {
            try
            {
                var userRoleIds = await dbContext.AppUserRoles
                    .Where(ur => ur.UserId == LoginUserId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();
                    

                var permissions = await dbContext.AppRolesScreens
                    .Where(rs => userRoleIds.Contains(rs.RoleId))
                    .Select(rs => new AppRolesScreenDto
                    {
                        Id = rs.Id,
                        RoleId = rs.RoleId,
                        ScreenId = rs.ScreenId,
                        CanView = rs.CanView,
                        CanAdd = rs.CanAdd,
                        CanEdit = rs.CanEdit,
                        CanDelete = rs.CanDelete
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user permissions for UserId: {UserId}", LoginUserId);
                throw new Exception(ex.Message);
            }
        }
    }

    public interface IAppRoleScreensService
    {
        Task<List<AppRolesScreenDto>> GetUserPermissionsList();
    }
}
