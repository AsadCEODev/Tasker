using Microsoft.EntityFrameworkCore;
using TMS.API.Data;
using TMS.API.Services.SetupServices;
using TMS.Shared.Enum;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Data;

namespace TSM.API.Services.SetupServices
{
    public class SetupDesignationService : BaseClassService, ISetupDesignationService
    {
        
        private readonly ILogger<SetupDesignationService> _logger;
        public SetupDesignationService(ApplicationDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<SetupDesignationService> logger) : base(dbContext, configuration, httpContextAccessor)
        {
            _logger = logger;
        }


        private IQueryable<SetupDesignation> BaseQuery(FilterModel filters)
        {
            try
            {
                var query = dbContext.SetupDesignations.AsQueryable();

                if (filters == null) return query;

                if (!string.IsNullOrWhiteSpace(filters.QueryString))
                {
                    var q = filters.QueryString.Trim().ToLower();
                    query = query.Where(x => x.DesignationName.ToLower().Contains(q));
                }


                if (!string.IsNullOrWhiteSpace(filters.IsActive))
                {
                    if (filters.IsActive == "Active")
                    {
                        query = query.Where(x => x.IsActive == true);
                    }
                    else if (filters.IsActive == "InActive")
                    {
                        query = query.Where(x => x.IsActive == false);
                    }
                }

                return query;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<PaginationResponse<SetupDesignation>> GetAll(FilterModel filter)
        {
            try
            {
                var query = BaseQuery(filter);
                int totalRecords = query.Count();


                var items = await query.OrderBy(x => x.Id)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return new PaginationResponse<SetupDesignation>
                {
                    Data = items,
                    PageIndex = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<SetupDesignation>> GetDesignationsList()
        {
            try
            {
                var lst = await dbContext.SetupDesignations.ToListAsync();
                return lst;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SetupDesignation> GetById(int id)
        {
            try
            {
                var found = await dbContext.SetupDesignations.FirstOrDefaultAsync(x => x.Id == id);
                return found;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> Save(SetupDesignation model)
        {
            try
            {
                var found = dbContext.SetupDesignations.FirstOrDefault(x => x.DesignationName.ToLower() == model.DesignationName.ToLower());
                if (found != null)
                {
                    return -1;
                }

                model.CreatedBy = base.LoginUserName;
                model.CreatedOn = DateTime.Now;
                dbContext.SetupDesignations.Add(model);
                await dbContext.SaveChangesAsync();
                return 1;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(SetupDesignation model)
        {
            try
            {
                var found = dbContext.SetupDesignations.FirstOrDefault(x => x.DesignationName.ToLower() == model.DesignationName.ToLower() && x.Id != model.Id);
                if (found != null)
                {
                    return -1;
                }
                var existing = await dbContext.SetupDesignations.FirstOrDefaultAsync(x => x.Id == model.Id);

                existing.DesignationName = model.DesignationName;
                existing.IsActive = model.IsActive;
                existing.UpdatedBy = base.LoginUserName;
                existing.UpdatedOn = DateTime.Now;
                
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var found = await dbContext.SetupDepartments.FirstOrDefaultAsync(x => x.Id == id);
                if (found == null)
                {
                    return false;
                }
                dbContext.SetupDepartments.Remove(found);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<vwDesignationSummaryData> GetDesignationSummary()
        {
            try
            {
                var data = await dbContext.GetDataFromView<vwDesignationSummaryData>("vwDesignationSummary_Data");
                return data;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

    public interface ISetupDesignationService
    {
        Task<PaginationResponse<SetupDesignation>> GetAll(FilterModel filter);
        Task<List<SetupDesignation>> GetDesignationsList();
        Task<SetupDesignation> GetById(int id);
        Task<int> Save(SetupDesignation model);
        Task<int> Update(SetupDesignation model);
        Task<bool> Delete(int id);
        Task<vwDesignationSummaryData> GetDesignationSummary();
    }
}
