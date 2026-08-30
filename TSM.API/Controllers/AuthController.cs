using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Auth;
using TMS.Shared.Model.Setup;
using TSM.API.Services;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService thisService;
        public AuthController(IAuthService service)
        {
            thisService = service;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await thisService.LoginAsync(model);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { Token = token });
        }

        [HttpGet("GetCurrentUserInfo")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            try
            {
                var result = await thisService.GetCurrentUserInfo();
                if(result == null)
                {
                    return Unauthorized();
                }
                var converted = result.Adapt<SetupUserDto>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
