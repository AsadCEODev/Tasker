using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TMS.API.Data;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMS.API.Services.SetupServices
{
    public class TaskService : BaseClassService, ITaskService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<TaskService> _logger;

        // Constructor mein base class aur local dependencies inject ki gayi hain
        public TaskService(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment,
            ILogger<TaskService> logger)
            : base(dbContext, configuration, httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
        }

        private IQueryable<SetupTask> GetBaseSetupTaskQuery()
        {
            return dbContext.SetupTasks
                .AsNoTracking()
                .Include(x => x.TagObj)
                .Include(x => x.StatusObj)
                .Include(x => x.ProjectObj)
                .Include(x => x.UserObj);
        }

        public async Task<PaginationResponse<SetupTask>> GetAll(FilterModel filter)
        {
            try
            {
                var query = GetBaseSetupTaskQuery();

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(x => x.DueDate.Date >= filter.FromDate.Value.Date);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(x => x.DueDate.Date <= filter.ToDate.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(filter.QueryString))
                {
                    var searchTerm = filter.QueryString.Trim().ToLower();

                    query = query.Where(x =>
                        (!string.IsNullOrEmpty(x.TaskTitle) && x.TaskTitle.ToLower().Contains(searchTerm)) ||
                        (!string.IsNullOrEmpty(x.TaskDesc) && x.TaskDesc.ToLower().Contains(searchTerm)) ||
                        (!string.IsNullOrEmpty(x.ProjectObj.ProjectName) && x.ProjectObj.ProjectName.ToLower().Contains(searchTerm)) ||
                        (!string.IsNullOrEmpty(x.UserObj.UserName) && x.UserObj.UserName.ToLower().Contains(searchTerm)) ||
                        (!string.IsNullOrEmpty(x.UserObj.FullName) && x.UserObj.FullName.ToLower().Contains(searchTerm)) ||
                        
                        (!string.IsNullOrEmpty(x.CreatedBy) && x.CreatedBy.ToLower().Contains(searchTerm))
                    );
                }

                if (filter.TagId != null && filter.TagId > 0)
                {
                    query = query.Where(x => x.TagId == filter.TagId);
                }

                if (filter.StatusId != null && filter.StatusId > 0)
                {
                    query = query.Where(x => x.StatusId == filter.StatusId);
                }

                var totalRecords = await query.CountAsync();

                var items = await query.OrderByDescending(x => x.Id)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return new PaginationResponse<SetupTask>
                {
                    Data = items,
                    PageIndex = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = totalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving setup tasks.");
                throw new Exception("An error occurred while retrieving setup tasks.", ex);
            }
        }

        public async Task<SetupTask?> GetById(long id)
        {
            try
            {
                return await GetBaseSetupTaskQuery()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting task by ID: {Id}", id);
                throw;
            }
        }

        public async Task<int> Save(SetupTask model, IFormFile? file)
        {
            try
            {
                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.TaskTitle.ToLower() == model.TaskTitle.ToLower() && x.ProjectId == model.ProjectId);
                if (found != null)
                {
                    _logger.LogWarning("Duplicate task title found during save: {TaskTitle}", model.TaskTitle);
                    return -1;
                }

                // 1. File Upload Logic (Unique Name + wwwroot/Image)
                if (file != null && file.Length > 0)
                {
                    // Aapka mukammal path yahan set kar diya gaya hai
                    string folderPath = @"D:\Tasker\TMS\wwwroot\Files";

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string extension = Path.GetExtension(file.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    model.FileName = uniqueFileName;
                }

                model.ProjectObj = null;
                model.StatusObj = null;
                model.TagObj = null;
                model.UserObj = null;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = LoginUserName; // Base class property se automatic name
                model.UserId = LoginUserId > 0 ? LoginUserId : model.UserId;

                dbContext.SetupTasks.Add(model);

                // 2. User Activity Log Entry
                dbContext.UserActivityLogs.Add(new UserActivityLog
                {
                    UserId = LoginUserId,
                    UserName = LoginUserName,
                    UserAction = "Create Task",
                    Details = $"Created task '{model.TaskTitle}'",
                    ActivityDateTime = DateTime.Now
                });

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Task saved successfully with Title: {TaskTitle}", model.TaskTitle);
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving task: {TaskTitle}", model.TaskTitle);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupTask model, IFormFile? file)
        {
            try
            {
                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (found == null)
                {
                    return 0;
                }

                bool isDuplicate = await dbContext.SetupTasks.AnyAsync(x => x.TaskTitle == model.TaskTitle && x.Id != model.Id && x.ProjectId == model.ProjectId);
                if (isDuplicate)
                {
                    return -1; // Duplicate title
                }

                string folderPath = @"D:\Tasker\TMS\wwwroot\Files";

                // File Update Logic (Purani delete & Nayi upload)
                if (file != null && file.Length > 0)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Agar pehle se koi file mojood thi toh usay delete kar dein
                    if (!string.IsNullOrEmpty(found.FileName))
                    {
                        string oldFilePath = Path.Combine(folderPath, found.FileName);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    string extension = Path.GetExtension(file.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    found.FileName = uniqueFileName;
                }
                else if (string.IsNullOrEmpty(model.FileName) && !string.IsNullOrEmpty(found.FileName))
                {
                    string oldFilePath = Path.Combine(folderPath, found.FileName);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                    found.FileName = null; // Database mein bhi null kar dein
                }

                found.TaskTitle = model.TaskTitle;
                found.TaskDesc = model.TaskDesc;
                found.DueDate = model.DueDate;
                found.TagId = model.TagId;
                found.StatusId = model.StatusId;
                found.UserId = model.UserId;
                found.UpdatedBy = LoginUserName;
                found.UpdatedOn = DateTime.Now;

                // User Activity Log Entry
                dbContext.UserActivityLogs.Add(new UserActivityLog
                {
                    UserId = LoginUserId,
                    UserName = LoginUserName,
                    UserAction = "Update Task",
                    Details = $"Updated task ID {model.Id} ('{model.TaskTitle}')",
                    ActivityDateTime = DateTime.Now
                });

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Task updated successfully with ID: {Id}", model.Id);
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task ID: {Id}", model.Id);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.Id == id);
                if (found == null)
                {
                    return false;
                }

                string folderPath = @"D:\Tasker\TMS\wwwroot\Files";
                if (!string.IsNullOrEmpty(found.FileName))
                {
                   
                    string filePath = Path.Combine(folderPath, found.FileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                dbContext.SetupTasks.Remove(found);

                // User Activity Log Entry
                dbContext.UserActivityLogs.Add(new UserActivityLog
                {
                    UserId = LoginUserId,
                    UserName = LoginUserName,
                    UserAction = "Delete Task",
                    Details = $"Deleted task ID {found.Id} ('{found.TaskTitle}')",
                    ActivityDateTime = DateTime.Now
                });

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Task deleted successfully with ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task ID: {Id}", id);
                throw new Exception(ex.Message);
            }
        }

        public async Task<TaskSummary> GetTasksSummary(FilterModel filter)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@UserId", LoginUserId },
                    { "@DateFrom", filter.FromDate },
                    { "@DateTo", filter.ToDate }
                };
                var summary = await dbContext.QueryFirstOrDefaultAsync<TaskSummary>("Proc_TaskSummary_Data", parameters);
                if(summary == null)
                {
                    return new();
                }
                return summary;
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ITaskService
    {
        Task<PaginationResponse<SetupTask>> GetAll(FilterModel filter);
        Task<SetupTask?> GetById(long id);
        Task<int> Save(SetupTask model, IFormFile? file);
        Task<int> Update(SetupTask model, IFormFile? file);
        Task<bool> Delete(long id);
        Task<TaskSummary> GetTasksSummary(FilterModel filter);
    }
}