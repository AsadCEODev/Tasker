using Mapster;
using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;

namespace TSM.API.Services.SetupServices
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext dbContext;
        public TagsService(ApplicationDbContext _dbContext)
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

        public async Task<List<SetupTag>> GetAll()
        {
            try
            {
                var lst = await dbContext.SetupTags.ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SetupTag> GetById(int id)
        {
            try
            {
                var found = await dbContext.SetupTags.FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }

    public interface ITagsService
    {
        Task<List<SetupTag>> GetAll();
        Task<SetupTag> GetById(int id);
    }
}
