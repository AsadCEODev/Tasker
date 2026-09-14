using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model;
using TSM.API.Services;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppScreenController : ControllerBase
    {
        private readonly IAppScreenService thisService;
        public AppScreenController(IAppScreenService service)
        {
            thisService = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var lst = await thisService.GetAll();
                if(lst == null)
                {
                    return NotFound(new());
                }
                var converted = lst.Adapt<List<AppScreenDto>>();
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
                if (found == null)
                {
                    return NotFound(new());
                }
                var converted = found.Adapt<AppScreenDto>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
