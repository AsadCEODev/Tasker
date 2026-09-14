using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Enum;
using TMS.Shared.Model;
using TSM.API.Data;
using TSM.API.Services.SetupServices;

namespace TSM.API.Services
{
    public class AppRolesService : BaseClassService, IAppRolesService
    {
        private readonly ILogger<SetupDepartmentService> _logger;
        public AppRolesService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<SetupDepartmentService> logger) : base(dbContext, configuration, httpContextAccessor)
        {
            _logger = logger;
        }


        public IQueryable<AppRole> BaseQuery()
        {
            return dbContext.AppRoles
                .Include(r => r.Permissions)
                 .ThenInclude(p => p.ScreenObject) 
                .AsNoTracking();
        }
        public async Task<List<AppRole>> GetAllAsync()
        {
            try
            {
                return await BaseQuery().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all roles.");
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> SaveOrUpdate(AppRole model)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

            try
            {
                
                var incomingPermissions = model.Permissions?.ToList() ?? new List<AppRolesScreen>();
                bool isInsert = model.Id == 0;

                if (isInsert)
                {

                    var existing = await dbContext.AppRoles.FirstOrDefaultAsync(x => x.RoleName == model.RoleName);
                    if (existing != null)
                    {
                        return (int)GenericRetureCodeEnum.Duplicate;
                    }

                    model.CreatedBy = base.LoginUserId;
                    model.CreatedOn = DateTime.Now;
                    model.Permissions = new List<AppRolesScreen>(); 

                    await dbContext.AppRoles.AddAsync(model);
                    await dbContext.SaveChangesAsync();

                    if (incomingPermissions.Any())
                    {
                        foreach (var permission in incomingPermissions)
                        {
                            permission.RoleId = model.Id;
                        }

                        await dbContext.AddRangeAsync(incomingPermissions);
                        await dbContext.SaveChangesAsync();
                    }

                    dbContext.UserActivityLogs.Add(new UserActivityLog
                    {
                        UserId = LoginUserId,
                        UserName = LoginUserName,
                        UserAction = "Create Role",
                        Details = $"Created role ID {model.Id} ('{model.RoleName}') with {incomingPermissions.Count} permissions.",
                        ActivityDateTime = DateTime.Now
                    });

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    var found = await dbContext.AppRoles
                        .Include(r => r.Permissions)
                        .FirstOrDefaultAsync(r => r.Id == model.Id);

                    if (found == null)
                    {
                        await transaction.RollbackAsync();
                        return (int)GenericRetureCodeEnum.NotFound;
                    }

                    string oldRoleName = found.RoleName;

                    found.RoleName = model.RoleName;
                    found.UpdatedBy = LoginUserId;
                    found.UpdatedOn = DateTime.Now;

                    var incomingIds = incomingPermissions.Where(p => p.Id > 0).Select(p => p.Id).ToList();
                    var permissionsToRemove = found.Permissions
                        .Where(p => !incomingIds.Contains(p.Id))
                        .ToList();

                    if (permissionsToRemove.Any())
                    {
                        dbContext.RemoveRange(permissionsToRemove);
                    }

                    foreach (var incomingPerm in incomingPermissions)
                    {
                        incomingPerm.RoleId = found.Id;
                        var existingPerm = found.Permissions.FirstOrDefault(p => p.Id == incomingPerm.Id && incomingPerm.Id > 0);

                        if (existingPerm == null)
                        {
                            found.Permissions.Add(incomingPerm);
                        }
                        else
                        {
                            existingPerm.ScreenId = incomingPerm.ScreenId;
                            existingPerm.CanView = incomingPerm.CanView;
                            existingPerm.CanAdd = incomingPerm.CanAdd;
                            existingPerm.CanEdit = incomingPerm.CanEdit;
                            existingPerm.CanDelete = incomingPerm.CanDelete;
                            //existingPerm.CanPrint = incomingPerm.CanPrint;
                        }
                    }

                    dbContext.AppRoles.Update(found);
                    await dbContext.SaveChangesAsync();

                    // --- USER ACTIVITY LOG (UPDATE) ---
                    dbContext.UserActivityLogs.Add(new UserActivityLog
                    {
                        UserId = LoginUserId,
                        UserName = LoginUserName,
                        UserAction = "Update Role",
                        Details = $"Updated role ID {found.Id} from '{oldRoleName}' to '{found.RoleName}'",
                        ActivityDateTime = DateTime.Now
                    });

                    await dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return (int)GenericRetureCodeEnum.Success;
            }
            catch ( Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
            
        }


    }

    public interface IAppRolesService
    {
        Task<int> SaveOrUpdate(AppRole model);
        Task<List<AppRole>> GetAllAsync();
    }
}
