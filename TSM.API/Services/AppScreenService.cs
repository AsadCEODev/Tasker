using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model;
using TSM.API.Data;
using TSM.API.Services.SetupServices;

namespace TSM.API.Services
{
    public class AppScreenService : BaseClassService, IAppScreenService
    {
        private readonly ILogger<SetupDepartmentService> _logger;
        public AppScreenService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<SetupDepartmentService> logger) : base(dbContext, configuration, httpContextAccessor)
        {
            _logger = logger;
        }


        public async Task<List<AppScreen>> GetAll()
        {
            try
            {
                var lst = await dbContext.AppScreens.ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<AppScreen> GetById(int id)
        {
            try
            {
                var found = await dbContext.AppScreens.FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface IAppScreenService
    {
        Task<List<AppScreen>> GetAll();
        Task<AppScreen> GetById(int id);
    }
}
