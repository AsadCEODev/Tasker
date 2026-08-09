using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TSM.API.Services.SetupServices
{
    public class AssignProjectService : IAssignProjectService
    {
        private readonly ApplicationDbContext dbContext;
        public AssignProjectService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<PaginationResponse<UserProject>> GetAll(int pageIndex, int pageSize, string queryString)
        {
            try
            {


                var query = dbContext.UserProjects.AsNoTracking()
                .Include(x => x.UserObj)
                .Include(x => x.ProjectObj)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    query = query.Where(u => u.ProjectObj.ProjectName.Contains(queryString)
                    || u.UserObj.UserName.Contains(queryString)

                    );
                }

                var totalCount = await query.CountAsync();

                // 4. Data fetch karein
                var data = await query
                    .OrderByDescending(u => u.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginationResponse<UserProject>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserProject>> GetByUserId(long userId)
        {
            try
            {
                var lst = await dbContext.UserProjects.AsNoTracking()
.Include(x => x.UserObj).Include(x => x.ProjectObj)
.Where(x => x.UserId == userId).ToListAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<UserProject> GetById(long id)
        {
            try
            {
                var found = await dbContext.UserProjects.AsNoTracking().Include(x => x.UserObj).Include(x => x.ProjectObj).FirstOrDefaultAsync(x => x.Id == id);
                return found;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Save(UserAssignProjectsDto dto)
        {
            try
            {
                dto.ProjectIds ??= new List<int>();

                var existingProjects = await dbContext.UserProjects.AsNoTracking().Include(x => x.UserObj).Include(x => x.ProjectObj)
                    .Where(x => x.UserId == dto.UserId)
                    .ToListAsync();

                foreach (var project in existingProjects)
                {
                    if (dto.ProjectIds.Contains(project.ProjectId))
                    {
                        project.IsActive = true;
                    }
                    else
                    {
                        project.IsActive = false;
                    }

                    project.UpdatedBy = "Admin";
                    project.UpdatedOn = DateTime.Now;
                }

                var existingProjectIds = existingProjects
                    .Select(x => x.ProjectId)
                    .ToHashSet();

                var newProjects = dto.ProjectIds
                    .Where(x => !existingProjectIds.Contains(x))
                    .Select(x => new UserProject
                    {
                        UserId = dto.UserId,
                        ProjectId = x,
                        IsActive = true,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now
                    });

                await dbContext.UserProjects.AddRangeAsync(newProjects);

                await dbContext.SaveChangesAsync();

                return true;
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
                var found = await dbContext.UserProjects.AsNoTracking().Include(x => x.UserObj).Include(x => x.ProjectObj).FirstOrDefaultAsync(x => x.Id == id);
                if (found == null)
                {
                    return false;
                }
                dbContext.UserProjects.Remove(found);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

    public interface IAssignProjectService
    {
        Task<PaginationResponse<UserProject>> GetAll(int pageIndex, int pageSize, string queryString);
        Task<UserProject> GetById(long id);
        Task<List<UserProject>> GetByUserId(long userId);
        Task<bool> Save(UserAssignProjectsDto dto);
        Task<bool> Delete(long id);
    }
}
