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
        private readonly ILogger<TaskService> _logger;

        // Constructor mein base class aur local dependencies inject ki gayi hain
        public TaskService( ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,ILogger<TaskService> logger): base(dbContext, configuration, httpContextAccessor)
        {
            
            _logger = logger;
        }

        private IQueryable<SetupTask> GetBaseSetupTaskQuery()
        {
            return dbContext.SetupTasks
                .AsNoTracking()
                .Include(x => x.TagObj)
                .Include(x => x.StatusObj)
                .Include(x => x.ProjectObj)
                .Include(x => x.UserTasks)
                    .ThenInclude(x => x.UserObj)
                    .AsNoTracking()
                .AsQueryable(); ;

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

        public async Task<PaginationResponse<SetupTask>> GetTasksByUser(FilterModel filter)
        {
            try
            {
                var query = GetBaseSetupTaskQuery();
                if(LoginUserId > 0)
                {
                    query = query.Where(x =>x.UserTasks.Any(u => u.UserId == LoginUserId));
                }
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
                // Duplicate task check
                var found = await dbContext.SetupTasks
                    .FirstOrDefaultAsync(x =>
                        x.TaskTitle.ToLower() == model.TaskTitle.ToLower() &&
                        x.ProjectId == model.ProjectId);

                if (found != null)
                {
                    _logger.LogWarning(
                        "Duplicate task title found during save: {TaskTitle}",
                        model.TaskTitle);

                    return -1;
                }
                if (file != null && file.Length > 0)
                {
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
                    model.UserFileName = file.FileName;
                }

                var assignedUserIds = model.UserTasks?
                    .Where(x => x.UserId > 0)
                    .Select(x => x.UserId)
                    .Distinct()
                    .ToList()
                    ?? new List<long>();

                model.ProjectObj = null;
                model.StatusObj = null;
                model.TagObj = null;

                model.UserTasks = new List<UserTask>();

                model.CreatedOn = DateTime.Now;
                model.CreatedBy = LoginUserName;

                dbContext.SetupTasks.Add(model);

                await dbContext.SaveChangesAsync();

                if (assignedUserIds.Any())
                {
                    var userTasks = assignedUserIds.Select(userId => new UserTask
                    {
                        UserId = userId,
                        TaskId = model.Id,
                        CreatedBy = LoginUserName,
                        CreatedOn = DateTime.Now
                    }).ToList();

                    await dbContext.UserTasks.AddRangeAsync(userTasks);
                }

                dbContext.UserActivityLogs.Add(new UserActivityLog
                {
                    UserId = LoginUserId,
                    UserName = LoginUserName,
                    UserAction = "Create Task",
                    Details = $"Created task '{model.TaskTitle}'",
                    ActivityDateTime = DateTime.Now
                });

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Task saved successfully. TaskId: {TaskId}, Title: {TaskTitle}, AssignedUsers: {UserCount}",
                    model.Id,
                    model.TaskTitle,
                    assignedUserIds.Count);

                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving task: {TaskTitle}",
                    model.TaskTitle);

                throw;
            }
        }

        public async Task<int> Update(SetupTask model, IFormFile? file)
        {
            try
            {
                var found = await dbContext.SetupTasks
                    .Include(x => x.UserTasks)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (found == null)
                {
                    return 0;
                }

                bool isDuplicate = await dbContext.SetupTasks
                    .AnyAsync(x => x.TaskTitle.ToLower() == model.TaskTitle.ToLower() && x.Id != model.Id && x.ProjectId == model.ProjectId);

                if (isDuplicate)
                {
                    return -1; 
                }

                string folderPath = @"D:\Tasker\TMS\wwwroot\Files";
                if (file != null && file.Length > 0)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

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
                    found.FileName = null;
                }

                found.TaskTitle = model.TaskTitle;
                found.TaskDesc = model.TaskDesc;
                found.DueDate = model.DueDate;
                found.TagId = model.TagId;
                found.StatusId = model.StatusId;
                found.TaskTime = model.TaskTime;
                found.IsStart = model.IsStart;
                found.UpdatedBy = LoginUserName;
                found.UpdatedOn = DateTime.Now;

                if (model.UserTasks != null)
                {
                  
                    var existingUserIds = found.UserTasks.Select(ut => ut.UserId).ToList();
                    var incomingUserIds = model.UserTasks.Select(ut => ut.UserId).ToList();

                    var usersToRemove = found.UserTasks.Where(ut => !incomingUserIds.Contains(ut.UserId)).ToList();
                    foreach (var removeTask in usersToRemove)
                    {
                        dbContext.UserTasks.Remove(removeTask);
                    }

      
                    foreach (var incomingTask in model.UserTasks)
                    {
                        if (!existingUserIds.Contains(incomingTask.UserId))
                        {
                            found.UserTasks.Add(new UserTask
                            {
                                TaskId = found.Id,
                                UserId = incomingTask.UserId,
                                CreatedBy = LoginUserName,
                                CreatedOn = DateTime.Now
                            });
                        }
                    }
                }
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
        public async Task<TaskSummary> GetUserTodoTasksSummary(FilterModel filter)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@UserId", LoginUserId },
                    { "@DateFrom", filter.FromDate },
                    { "@DateTo", filter.ToDate }
                };
                var summary = await dbContext.QueryFirstOrDefaultAsync<TaskSummary>("Proc_UserTodoTaskSummary_Data", parameters);
                if (summary == null)
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
        Task<PaginationResponse<SetupTask>> GetTasksByUser(FilterModel filter);
        Task<SetupTask?> GetById(long id);
        Task<int> Save(SetupTask model, IFormFile? file);
        Task<int> Update(SetupTask model, IFormFile? file);
        Task<bool> Delete(long id);
        Task<TaskSummary> GetTasksSummary(FilterModel filter);
        Task<TaskSummary> GetUserTodoTasksSummary(FilterModel filter);
    }
}