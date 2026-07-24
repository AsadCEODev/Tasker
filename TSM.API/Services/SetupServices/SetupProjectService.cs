using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;

namespace TSM.API.Services.SetupServices
{
    public class SetupProjectService : ISetupProjectService
    {
        private readonly ApplicationDbContext dbContext;

        public SetupProjectService(ApplicationDbContext _dbContext)
        {
            try
            {
                dbContext = _dbContext;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SetupProject>> GetAll()
        {
            try
            {
                var lst = await dbContext.SetupProjects.ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupProject> GetById(int id)
        {
            try
            {
                var found = await dbContext.SetupProjects.FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<SetupProject>> GetActiveList()
        {
            try
            {
                var lst = await dbContext.SetupProjects.Where(x => x.IsActive == true).ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ISetupProjectService
    {
        Task<List<SetupProject>> GetAll();
        Task<SetupProject> GetById(int id);
        Task<List<SetupProject>> GetActiveList();
    }
}
