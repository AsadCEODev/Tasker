using Mapster;
using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Enum;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TSM.API.Data;
using TSM.API.Services.SetupServices;

namespace TSM.API.Services
{
    public class AppRolesService : BaseClassService, IAppRolesService
    {
        private readonly ILogger<AppRolesService> _logger;
        public AppRolesService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<AppRolesService> logger) : base(dbContext, configuration, httpContextAccessor)
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

        public async Task<List<AppRole>> GetRolesList()
        {
            try
            {
                var query = BaseQuery();
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching app roles screens.");
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AppRole>> GetAllAsync(FilterModel filters)
        {
            try
            {
                var query = BaseQuery();
                if (!string.IsNullOrWhiteSpace(filters?.QueryString))
                {
                    var searchTerm = filters.QueryString.Trim();
                    query = query.Where(x => x.RoleName.Contains(searchTerm));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching app roles screens.");
                throw new Exception(ex.Message);
            }
        }
        public async Task<AppRole> GetById(long id)
        {
            try
            {
                var query = BaseQuery();
                var data = await query.FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching app roles screens.");
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> SaveOrUpdate(AppRole model)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

            try
            {
                var incomingPermissions = model.Permissions?
                    .Where(p => p.CanView || p.CanAdd || p.CanEdit || p.CanDelete)
                    .ToList() ?? new List<AppRolesScreen>();

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
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var found = await dbContext.AppRoles.FirstOrDefaultAsync(x => x.Id == id);
                if(found == null)
                {
                    return false;
                }
                dbContext.AppRoles.Remove(found);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }

    public interface IAppRolesService
    {
        Task<List<AppRole>> GetRolesList();
        Task<List<AppRole>> GetAllAsync(FilterModel filters);
        Task<AppRole> GetById(long id);
        Task<int> SaveOrUpdate(AppRole model);
        Task<bool> Delete(long id);
    }
}
