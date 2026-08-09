using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;

namespace TSM.API.Services.SetupServices
{
    public class SetupDepartmentService : ISetupDepartmentService
    {
        private readonly ApplicationDbContext dbContext;
        public SetupDepartmentService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<SetupDepartment>> GetAll()
        {
            try
            {
                var lst = await dbContext.SetupDepartments.ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupDepartment> GetById(int id)
        {
            try
            {
                var found = await dbContext.SetupDepartments.FirstOrDefaultAsync(x => x.Id == id);
                return found;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }        
        }
        public async Task<int> Save(SetupDepartment model)
        {
            try
            {
                var found = dbContext.SetupDepartments.FirstOrDefault(x => x.DepartmentonName.ToLower() == model.DepartmentonName.ToLower());
                if(found != null)
                {
                    return -1;
                }
                dbContext.SetupDepartments.Add(model);
                await dbContext.SaveChangesAsync();
                return 0;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupDepartment model)
        {
            try
            {
                var found = dbContext.SetupDepartments.FirstOrDefault(x => x.DepartmentonName.ToLower() == model.DepartmentonName.ToLower() && x.Id != model.Id);
                if (found != null)
                {
                    return -1;
                }

                found.DepartmentonName = model.DepartmentonName;
                
                await dbContext.SaveChangesAsync();
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public interface ISetupDepartmentService
    {
    }
}
