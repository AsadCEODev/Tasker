using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TMS.API.Data;
using TMS.API.Services.SetupServices;
using TMS.Shared.Enum;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Data;

namespace TSM.API.Services
{
    public class UserAssignedTasksService : BaseClassService, IUserAssignedTasksService
    {
        private readonly ILogger<TaskService> _logger;
        public UserAssignedTasksService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<TaskService> logger) : base(dbContext, configuration, httpContextAccessor)
        {

            _logger = logger;
        }
        private IQueryable<UserTask> BaseQuery()
        {
            return dbContext.UserTasks
                .Include(x => x.TaskObj)
                .Include(x => x.UserObj)
                .AsNoTracking()
                .AsQueryable();
        }
        public async Task<PaginationResponse<UserTask>> GetAll(FilterModel filter)
        {
            try
            {
                var query = BaseQuery();
                var totalCount = await query.CountAsync();

                var lst = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return new PaginationResponse<UserTask>
                {
                    PageIndex = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = totalCount,
                    Data = lst
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserTask> GetById(long id)
        {
            try
            {
                var found = await dbContext.UserTasks.FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationResponse<UserTaskData>> GetAllByUser(FilterModel filter)
        {
            try
            {
                long targetUserId = filter.UserId ?? LoginUserId;

                var parameters = new Dictionary<string, object>
                {
                    { "@UserId", LoginUserId },
                    { "@DateFrom", filter.FromDate },
                    { "@DateTo", filter.ToDate }
                };
                var lst = await dbContext.QueryListAsync<UserTaskData>("Proc_UserTask_Data", parameters);
                var tasklist = lst.ToList();

                var totalCount = tasklist.Count;
                
                var pagedData = tasklist
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                return new PaginationResponse<UserTaskData>
                {
                    PageIndex = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = totalCount,
                    Data = pagedData 
                };


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(UserTask task, IFormFile? uploadedFile)
        {
            try
            {
                var existing = await dbContext.UserTasks.FirstOrDefaultAsync(x => x.UserId == LoginUserId && x.TaskId == task.TaskId);
                if (existing == null)
                    return false;

                existing.Remarks = task.Remarks;
                existing.UserProgress = task.UserProgress;
                existing.UpdatedOn = DateTime.Now;
                string folderPath = @"D:\Tasker\TMS\wwwroot\Files";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (uploadedFile != null)
                {
                    if (!string.IsNullOrEmpty(existing.UserFileName))
                    {
                        var oldFilePath = Path.Combine(folderPath, existing.UserFileName);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    var fileName = $"{Guid.NewGuid()}_{uploadedFile.Name}";
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.OpenReadStream().CopyToAsync(stream);
                    }

                    existing.UserFileName = fileName;
                }
                else if (string.IsNullOrEmpty(task.UserFileName))
                {
                    if (!string.IsNullOrEmpty(existing.UserFileName))
                    {
                        var oldFilePath = Path.Combine(folderPath, existing.UserFileName);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    existing.UserFileName = null;
                }

                await dbContext.SaveChangesAsync();
                await UpdateMainTaskProgressAsync(task.TaskId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user task: {ex.Message}");
            }
        }

        private async Task UpdateMainTaskProgressAsync(long taskId)
        {
            var allUserTasks = await dbContext.UserTasks
                .Where(ut => ut.TaskId == taskId)
                .ToListAsync();

            if (allUserTasks.Any())
            {
                int averageProgress = (int)allUserTasks.Average(x => x.UserProgress);

                var mainTask = await dbContext.SetupTasks.FindAsync(taskId);
                if (mainTask != null)
                {
                    

                    if (allUserTasks.All(x => x.UserProgress >= 100))
                    {

                        mainTask.StatusId = (int)StatusEnum.Completed;
                        mainTask.Progress = 100;
                    }
                    else
                    {
                        mainTask.StatusId = (int)StatusEnum.Inprocess;
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
        public async Task<bool> ChangeStatus(UserTask model)
        {
            try
            {
                var found = await dbContext.UserTasks.FirstOrDefaultAsync(x => x.TaskId == model.TaskId && x.UserId == LoginUserId); 

                if (found == null) return false;

                if (model.IsStart)
                {
                    var runningTask = await dbContext.UserTasks.Where(x => x.UserId == LoginUserId && x.IsStart == true && x.TaskId != model.Id).FirstOrDefaultAsync();

                    if (runningTask != null)
                    {
                        return false; 
                    }
                }

                found.StatusId = (int)StatusEnum.Inprocess;
                found.TaskTime = model.TaskTime;
                found.IsStart = model.IsStart;

                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

    public interface IUserAssignedTasksService
    {
        Task<PaginationResponse<UserTask>> GetAll(FilterModel filter);
        Task<UserTask> GetById(long id);
        Task<PaginationResponse<UserTaskData>> GetAllByUser(FilterModel filter);
        Task<bool> Update(UserTask task, IFormFile? uploadedFile);
        Task<bool> ChangeStatus(UserTask model);
    }

}