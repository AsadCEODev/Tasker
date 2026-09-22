using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSM.API.Services;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppRoleScreensController : ControllerBase
    {
        private readonly IAppRoleScreensService thisService;
        public AppRoleScreensController(IAppRoleScreensService service)
        {
            thisService = service;
        }

        [HttpGet("GetUserPermissions")]
        public async Task<IActionResult> GetUserPermissions()
        {
            try
            {
                var lst = await thisService.GetUserPermissionsList();
                if(lst == null)
                {
                    return NotFound(new());
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }

}
