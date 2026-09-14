using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Services.SetupServices;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly ISetupDesignationService thisService;
        public DesignationsController(ISetupDesignationService _thisService)
        {
            thisService = _thisService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(FilterModel filter)
        {
            try
            {
                var lst = await thisService.GetAll(filter);
                if(lst == null)
                {
                    return NotFound();
                }
                var converted = lst.Adapt<PaginationResponse<SetupDesignationDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetDesignationsList")]
        public async Task<IActionResult> GetDesignationsList()
        {
            try
            {
                var lst = await thisService.GetDesignationsList();
                if (lst == null)
                {
                    return NotFound(lst);
                }
                var converted = lst.Adapt<List<SetupDesignationDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var found = await thisService.GetById(id);
                if (found == null)
                {
                    return NotFound();
                }
                var converted = found.Adapt<SetupDesignationDto>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(SetupDesignationDto dto)
        {
            try
            {
                var converted = dto.Adapt<SetupDesignation>();
                var result = await thisService.Save(converted);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update(SetupDesignationDto dto)
        {
            try
            {
                var converted = dto.Adapt<SetupDesignation>();
                var result = await thisService.Update(converted);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await thisService.Delete(id);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("GetDesignationSummary")]
        public async Task<IActionResult> GetDesignationSummary()
        {
            try
            {
                var dala = await thisService.GetDesignationSummary();
                if (dala == null)
                {
                    return NotFound(new());
                }
                var converted = dala.Adapt<vwDesignationSummaryDataDto>();
                return Ok(converted);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
