using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Setup;
using TSM.API.Services.SetupServices;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService thisService;
        public StatusController(IStatusService service)
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
                var converted = result.Adapt<List<SetupStatusDto>>();
               if(converted != null)
                {
                    return Ok(converted);
                }
               return BadRequest(converted);
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
                var result = await thisService.GetById(id);
                var converted = result.Adapt<SetupStatusDto>();
                if (converted != null)
                {
                    return Ok(converted);
                }
                return BadRequest(converted);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
