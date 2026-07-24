using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using Mapster;

namespace TMS.API.Services.SetupServices
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext dbContext;
        
        public TaskService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }




        // Common Base Query Method
        private IQueryable<SetupTask> GetBaseSetupTaskQuery()
        {
            return dbContext.SetupTasks
                .AsNoTracking()
                .Include(x => x.TagObj)
                .Include(x => x.StatusObj)
                .Include(x => x.ProjectObj)
                .Include(x => x.UserObj);
        }

        public async Task<PaginationResponse<SetupTaskDto>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var query = GetBaseSetupTaskQuery();

                var totalRecords = await query.CountAsync();

                var items = await query.OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectToType<SetupTaskDto>()
                    .ToListAsync();

                return new PaginationResponse<SetupTaskDto>
                {
                    Data = items,
                    PageIndex = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving setup tasks.", ex);
            }
        }

        public async Task<SetupTaskDto?> GetById(long id)
        {
            try
            {
                return await GetBaseSetupTaskQuery()
                    .Where(x => x.Id == id)
                    .ProjectToType<SetupTaskDto>()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> Save(SetupTask model)
        {
            try
            {

                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.TaskTitle.ToLower() == model.TaskTitle.ToLower() && x.ProjectId == model.ProjectId);
                if (found != null)
                {
                    return -1;
                }
                model.ProjectObj = null;
                model.StatusObj = null;
                model.TagObj = null;
                model.UserObj = null;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = "Admin";
                dbContext.SetupTasks.Add(model);
                await dbContext.SaveChangesAsync();
                return 1;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupTask model)
        {
            try
            {
                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.Id ==  model.Id);
                if(found == null)
                {
                    return 0;
                }
                found.TaskDesc = model.TaskDesc;
                found.DueDate = model.DueDate;
                found.TagId = model.TagId;
                found.StatusId = model.StatusId;
                found.UserId = model.UserId ?? null;
                found.UpdatedBy ="Admin";
                found.UpdatedOn = DateTime.Now;

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
                var found = dbContext.SetupTasks.FirstOrDefault(x => x.Id == id);
                if( found == null)
                {
                    return false;
                }

                dbContext.SetupTasks.Remove(found);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ITaskService
    {
        Task<PaginationResponse<SetupTaskDto>> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<SetupTaskDto> GetById(long id);
        Task<int> Save(SetupTask model);
        Task<int> Update(SetupTask model);
        Task<bool> Delete(long id);
    }
}
