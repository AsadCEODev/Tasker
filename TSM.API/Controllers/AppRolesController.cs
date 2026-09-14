using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Enum;
using TMS.Shared.Model;
using TSM.API.Services;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppRolesController : ControllerBase
    {
        private readonly IAppRolesService thisService;
        public AppRolesController(IAppRolesService service)
        {
            thisService = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await thisService.GetAllAsync();
                return (IActionResult)data;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("SaveOrUpdate")]
        public async Task<int> SaveOrUpdate(AppRoleDto dto)
        {
            try
            {
                var converted = dto.Adapt<AppRole>();
                var result = await thisService.SaveOrUpdate(converted);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
