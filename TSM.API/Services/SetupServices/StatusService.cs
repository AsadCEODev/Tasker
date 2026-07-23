using Mapster;
using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;

namespace TSM.API.Services.SetupServices
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext dbContext;
        public StatusService(ApplicationDbContext _dbContext)
        {
            try
            {
                dbContext= _dbContext;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                 
        }

        public async Task<List<SetupStatus>> GetAll()
        {
            try
            {
                var lst = await dbContext.SetupStatuses.ToListAsync();
           
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupStatus> GetById(int id)
        {
            try
            {
                var found = await dbContext.SetupStatuses.FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }

    public interface IStatusService
    {
        Task<List<SetupStatus>> GetAll();
        Task<SetupStatus> GetById(int id);
    }
}
