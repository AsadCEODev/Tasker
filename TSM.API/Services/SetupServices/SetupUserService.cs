using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.API.Services.SetupServices
{
    public class SetupUserService: ISetupUserService
    {
        private readonly ApplicationDbContext dbContext;
        public SetupUserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private IQueryable<SetupUser> BaseQuery()
        {
            var query = dbContext.SetupUsers
                .Include(x => x.DepartmentObj)
                .Include(y => y.DesignationObj)
                .AsNoTracking();

            return query;
        }

        private IQueryable<SetupUser> FilteredQuery(string? queryString)
        {
            // BaseQuery se starting query lein jismein includes aur tracking settings pehle se hon
            var query = BaseQuery();

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                query = query.Where(u =>
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.Contains(queryString)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(queryString)) ||
                    (!string.IsNullOrEmpty(u.PhoneNo) && u.PhoneNo.Contains(queryString)) ||
                    (!string.IsNullOrEmpty(u.FullName) && u.FullName.Contains(queryString)) ||
                    (!string.IsNullOrEmpty(u.CNIC) && u.CNIC.Contains(queryString))
                );
            }

            return query;
        }

        public async Task<PaginationResponse<SetupUser>> GetAll(int pageIndex, int pageSize, string? queryString)
        {
            try
            {
                // 1. Base query define karein
                var query = FilteredQuery(queryString);

                // 3. Pehle TotalCount nikalein (filtered data ka)
                var totalCount = await query.CountAsync();

                // 4. Data fetch karein
                var data = await query
                    .OrderByDescending(u => u.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginationResponse<SetupUser>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Data = data
                };
            }
            catch (Exception)
            {
                // Exception re-throw karein taake stack trace zaya na ho
                throw;
            }
        }
        public async Task<List<SetupUser>> GetUsersList()
        {
            try
            {
                var list = await BaseQuery().ToListAsync();
                return list;
            }
            catch (Exception)
            {
                // Exception re-throw karein taake stack trace zaya na ho
                throw;
            }
        }



        public async Task<SetupUser> GetById(long id)
        {
            try
            {
                var result = await BaseQuery().FirstOrDefaultAsync(x => x.Id == id);
                if (result == null)
                {
                    return new SetupUser();
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> Save(SetupUser model)
        {
            try
            {
                var existingUser = await dbContext.SetupUsers.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (existingUser != null)
                {
                    return -1; // User already exists
                }
                model.HashPassword = BCrypt.Net.BCrypt.HashPassword(model.HashPassword);

                dbContext.SetupUsers.Add(model);
                await dbContext.SaveChangesAsync();
                return 1; // User created successfully
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<int> Update(SetupUser model)
        {
            try
            {
                var existingUser = await dbContext.SetupUsers.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (existingUser == null)
                {
                    return -1; // User not found
                }
                existingUser.FullName = model.FullName;
                existingUser.PhoneNo = model.PhoneNo;
                existingUser.Email = model.Email;
                existingUser.IsActive = model.IsActive;
                existingUser.DepartmentId = model.DepartmentId;
                existingUser.DesignationId = model.DesignationId;
                existingUser.UpdatedBy = model.UpdatedBy;
                existingUser.UpdatedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return 1; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(long id)
        {
            try
            {
                var existingUser = await dbContext.SetupUsers.FirstOrDefaultAsync(u => u.Id == id);
                if (existingUser == null)
                {
                    return false; // User not found
                }
                dbContext.SetupUsers.Remove(existingUser);
                await dbContext.SaveChangesAsync();
                return true; // User deleted successfully
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdatePassword(long userId, string newPassword)
        {
            try
            {
                var existingUser = await dbContext.SetupUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (existingUser == null)
                {
                    return false; // User not found
                }
                existingUser.HashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                existingUser.UpdatedOn = DateTime.Now;
                await dbContext.SaveChangesAsync();
                return true; // Password updated successfully
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserSummary> GetUsersSummary()
        {
            try
            {
                var query = dbContext.SetupUsers.AsQueryable();

                return await query.GroupBy(x => 1) 
                    .Select(g => new UserSummary
                    {
                        TotalUserCount = g.LongCount(),
                        ActiveUserCount = g.LongCount(x => (bool)x.IsActive),
                        InactiveUserCount = g.LongCount(x => !(bool)x.IsActive)
                    })
                    .FirstOrDefaultAsync() ?? new UserSummary();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user summary.", ex);
            }
        }

    }

    public interface ISetupUserService
    {
        Task<PaginationResponse<SetupUser>> GetAll(int pageIndex, int pageSize,string? queryString);
        Task<List<SetupUser>> GetUsersList();
        Task<SetupUser> GetById(long id);
        Task<int> Save(SetupUser model);
        Task<int> Update(SetupUser model);
        Task<bool> Delete(long id);
        Task<bool> UpdatePassword(long userId, string newPassword);
        Task<UserSummary> GetUsersSummary();
    }
}
