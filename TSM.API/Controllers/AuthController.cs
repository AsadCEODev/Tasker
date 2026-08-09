using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Auth;
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


    }
}
