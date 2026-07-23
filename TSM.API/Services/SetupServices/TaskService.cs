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


       

        public async Task<PaginationResponse<SetupTaskDto>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalRecords = await dbContext.SetupTasks.CountAsync();

                var lst = await dbContext.SetupTasks
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var items = lst.Adapt<List<SetupTaskDto>>();
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

        public async Task<SetupTaskDto> GetById(long id)
        {
            try
            {
                var found = await dbContext.SetupTasks.FirstOrDefaultAsync(x => x.Id == id);
                return found.Adapt<SetupTaskDto>() ?? new();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Save(SetupTask model)
        {
            try
            {

                var found = dbContext.SetupTasks.FirstOrDefaultAsync(x => x.TaskTitle.ToLower() == model.TaskTitle.ToLower());
                if (found != null)
                {
                    return -1;
                }

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
