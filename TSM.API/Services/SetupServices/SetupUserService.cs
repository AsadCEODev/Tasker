using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.AppConstants;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Data;

namespace TMS.API.Services.SetupServices
{
    public class SetupUserService: BaseClassService,ISetupUserService
    {
        

        private readonly ILogger<TaskService> _logger;
        public SetupUserService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<TaskService> logger) : base(dbContext, configuration, httpContextAccessor)
        {

            _logger = logger;
        }
        private IQueryable<SetupUser> BaseQuery()
        {
            var query = dbContext.SetupUsers
                .Include(x => x.DepartmentObj)
                .Include(y => y.DesignationObj)
                .AsNoTracking();

            return query;
        }

        private IQueryable<SetupUser> FilteredQuery(FilterModel filters)
        {
            var query = BaseQuery();

            if (!string.IsNullOrWhiteSpace(filters.QueryString))
            {
                query = query.Where(u =>
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.Contains(filters.QueryString)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(filters.QueryString)) ||
                    (!string.IsNullOrEmpty(u.PhoneNo) && u.PhoneNo.Contains(filters.QueryString)) ||
                    (!string.IsNullOrEmpty(u.FullName) && u.FullName.Contains(filters.QueryString)) ||
                    (!string.IsNullOrEmpty(u.CNIC) && u.CNIC.Contains(filters.QueryString))
                );
            }

            query = (filters.ColumnName?.ToLower(), filters.SortType?.ToUpper()) switch
            {
                ("username", "DESC") => query.OrderByDescending(u => u.UserName),
                ("username", _) => query.OrderBy(u => u.UserName),

                ("fullname", "DESC") => query.OrderByDescending(u => u.FullName),
                ("fullname", _) => query.OrderBy(u => u.FullName),

                ("fathername", "DESC") => query.OrderByDescending(u => u.FatherName),
                ("fathername", _) => query.OrderBy(u => u.FatherName),

                ("email", "DESC") => query.OrderByDescending(u => u.Email),
                ("email", _) => query.OrderBy(u => u.Email),

                ("phoneno", "DESC") => query.OrderByDescending(u => u.PhoneNo),
                ("phoneno", _) => query.OrderBy(u => u.PhoneNo),

                ("isactive", "DESC") => query.OrderByDescending(u => u.IsActive),
                ("isactive", _) => query.OrderBy(u => u.IsActive),

                ("id", "DESC") => query.OrderByDescending(u => u.Id),
                _ => query.OrderBy(u => u.Id) // Default fallback sorting
            };

            if (!string.IsNullOrWhiteSpace(filters.IsActive))
            {
                if (filters.IsActive == "Active")
                {
                    query = query.Where(x => x.IsActive == true);
                }
                else if (filters.IsActive == "InActive")
                {
                    query = query.Where(x => x.IsActive == false);
                }
            } 
            return query;
        }

        public async Task<PaginationResponse<SetupUser>> GetAll(FilterModel filters)
        {
            try
            {
                var query = FilteredQuery(filters);

                var totalCount = await query.CountAsync();

                // 4. Data fetch karein
                var data = await query
                    
                    .Skip((filters.PageNumber - 1) * filters.PageSize)
                    .Take(filters.PageSize)
                    .ToListAsync();

                return new PaginationResponse<SetupUser>
                {
                    PageIndex = filters.PageNumber,
                    PageSize = filters.PageSize,
                    TotalCount = totalCount,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<SetupUser>> GetUsersList()
        {
            try
            {
                var list = await BaseQuery().ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    return -1; 
                }
                model.HashPassword = BCrypt.Net.BCrypt.HashPassword(model.HashPassword);

                dbContext.SetupUsers.Add(model);
                await dbContext.SaveChangesAsync();
                return 1; 
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
                existingUser.ProfileImagePath = model.ProfileImagePath;
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

        public async Task<int> UpdateProfile(SetupUser model, IFormFile? imageFile)
        {
            try
            {
                var existingUser = await dbContext.SetupUsers
                    .FirstOrDefaultAsync(u => u.Id == model.Id);

                if (existingUser == null)
                {
                    return -1;
                }

                // Old image ka path save kar lein
                var oldFileName = existingUser.ProfileImagePath;

                if (imageFile != null)
                {
                    // Files folder create karein agar exist nahi karta
                    if (!Directory.Exists(AppConstants.FilePath))
                    {
                        Directory.CreateDirectory(AppConstants.FilePath);
                    }

                    // Unique filename
                    var extension = Path.GetExtension(imageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";

                    var physicalPath = Path.Combine(
                        AppConstants.FilePath,
                        uniqueFileName);

                    // New image save
                    await using (var fileStream = new FileStream(
                        physicalPath,
                        FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // DB mein new filename
                    existingUser.ProfileImagePath = uniqueFileName;

                    // Purani image delete
                    if (!string.IsNullOrWhiteSpace(oldFileName))
                    {
                        var oldPhysicalPath = Path.Combine(
                            AppConstants.FilePath,
                            oldFileName);

                        if (File.Exists(oldPhysicalPath))
                        {
                            File.Delete(oldPhysicalPath);
                        }
                    }
                }
                else
                {
                    // =================================================
                    // FILE SELECT NAHI KI
                    // Purani image delete karein
                    // =================================================

                    if (!string.IsNullOrWhiteSpace(oldFileName))
                    {
                        var oldPhysicalPath = Path.Combine(
                            AppConstants.FilePath,
                            oldFileName);

                        if (File.Exists(oldPhysicalPath))
                        {
                            File.Delete(oldPhysicalPath);
                        }
                    }

                    // DB se filename remove
                    existingUser.ProfileImagePath = null;
                }

                // =====================================================
                // USER INFORMATION
                // =====================================================

                existingUser.FullName = model.FullName;
                existingUser.PhoneNo = model.PhoneNo;
                existingUser.Email = model.Email;
                existingUser.IsActive = model.IsActive;
                existingUser.DepartmentId = model.DepartmentId;
                existingUser.DesignationId = model.DesignationId;
                existingUser.UpdatedBy = LoginUserName;
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
                    return false; 
                }
                dbContext.SetupUsers.Remove(existingUser);
                await dbContext.SaveChangesAsync();
                return true; 
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
                    return false; 
                }
                existingUser.HashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                existingUser.UpdatedOn = DateTime.Now;
                await dbContext.SaveChangesAsync();
                return true; 
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
        Task<PaginationResponse<SetupUser>> GetAll(FilterModel filters);
        Task<List<SetupUser>> GetUsersList();
        Task<SetupUser> GetById(long id);
        Task<int> Save(SetupUser model);
        Task<int> Update(SetupUser model);
        Task<int> UpdateProfile(SetupUser model, IFormFile imageFile);
        Task<bool> Delete(long id);
        Task<bool> UpdatePassword(long userId, string newPassword);
        Task<UserSummary> GetUsersSummary();
    }
}
