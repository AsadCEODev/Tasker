using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Setup;
using TSM.API.Services.SetupServices;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ISetupProjectService thisService;
        public ProjectsController(ISetupProjectService service)
        {
            try
            {
                thisService = service;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await thisService.GetAll();
                if(result == null)
                {
                    return NotFound(result);
                }
                var converted = result.Adapt<List<SetupProjectDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var found = await thisService.GetById(id);
                if(found == null)
                {
                    return NotFound(found);
                }
                var converted = found.Adapt<SetupProjectDto>();
                return Ok(converted);
             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetActiveList")]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                var lst = await thisService.GetActiveList();
                if (lst == null)
                {
                    return NotFound(lst);
                }
                var converted = lst.Adapt<List<SetupProjectDto>>();
                return Ok(converted);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
