using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Pagination;
using TSM.API.Data;
using TSM.API.Services.SetupServices;

namespace TSM.API.Services
{
    public class AppUserRoleService : BaseClassService, IAppUserRoleService
    {
        private readonly ILogger<AppUserRoleService> _logger;
        public AppUserRoleService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<AppUserRoleService> logger) : base(dbContext, configuration, httpContextAccessor)
        {
            _logger = logger;
        }


        private IQueryable<AppUserRole> BaseQuery()
        {
            
            var query = dbContext.AppUserRoles
                .Include(x => x.UserObject)
                .Include(y => y.RoleObject)
                .AsNoTracking();

            return query;
        }



        public async Task<PaginationResponse<AppUserRole>> GetAll(FilterModel filters)
        {
            try
            {
                var query = BaseQuery();

                

                if (filters.RoleId.HasValue && filters.RoleId.Value > 0)
                {
                    query = query.Where(x => x.RoleId == filters.RoleId.Value);
                }

                if (filters.UserId.HasValue && filters.UserId.Value > 0)
                {
                    query = query.Where(x => x.UserId == filters.UserId.Value);
                }
                if (!string.IsNullOrWhiteSpace(filters.QueryString))
                {
                    query = query.Where(x =>
                        x.RoleObject!.RoleName.Contains(filters.QueryString) ||
                        x.UserObject!.FullName.Contains(filters.QueryString) ||
                        x.UserObject!.UserName.Contains(filters.QueryString));
                }
                var totalRecords = await query.CountAsync();
                var lst = await query
                    .OrderByDescending(x => x.Id)
                    .Skip((filters.PageNumber - 1) * filters.PageSize)
                    .Take(filters.PageSize)
                    .ToListAsync();

                return new PaginationResponse<AppUserRole>
                {
                    Data = lst,
                    TotalCount = totalRecords,
                    PageIndex = filters.PageNumber,
                    PageSize = filters.PageSize
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> SaveOrUpdateUsersRoleAsync(AppUserRole model)
        {
            try
            {
                var existingRecords = await dbContext.AppUserRoles.Where(x => x.RoleId == model.RoleId).ToListAsync();

                var incomingUserIds = model.UserIds.Distinct().ToList();

                var recordsToRemove = existingRecords.Where(x => !incomingUserIds.Contains(x.UserId)).ToList();

                if (recordsToRemove.Any())
                {
                    dbContext.AppUserRoles.RemoveRange(recordsToRemove);
                }

                var existingUserIds = existingRecords.Select(x => x.UserId).ToHashSet();

                var userIdsToAdd = incomingUserIds.Where(x => !existingUserIds.Contains(x)).ToList();

                var recordsToAdd = userIdsToAdd
                    .Select(userId => new AppUserRole
                    {
                        UserId = userId,
                        RoleId = model.RoleId,
                        CreatedBy = LoginUserId,
                        CreatedOn = DateTime.Now
                    })
                    .ToList();

                if (recordsToAdd.Any())
                {
                    await dbContext.AppUserRoles.AddRangeAsync(recordsToAdd);
                }

                await dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> Delete(long id)
        {
            try
            {
                var found = await dbContext.AppUserRoles.FirstOrDefaultAsync(x => x.Id == id);
                if( found == null)
                {
                    return false;
                }

                dbContext.AppUserRoles.Remove(found);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface IAppUserRoleService
    {
        Task<PaginationResponse<AppUserRole>> GetAll(FilterModel filters);
        Task<bool> SaveOrUpdateUsersRoleAsync(AppUserRole model);
        Task<bool> Delete(long id);
    }
}
