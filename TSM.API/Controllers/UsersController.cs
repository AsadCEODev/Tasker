using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.Services.SetupServices;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISetupUserService thisService;
        public UsersController(ISetupUserService service)
        {
            thisService = service;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize, string? queryString)
        {
            var pagedData = await thisService.GetAll(pageIndex,pageSize, queryString);
            
            var result = new PaginationResponse<SetupUserDto>
            {
                PageIndex = pagedData.PageIndex,
                PageSize = pagedData.PageSize,
                TotalCount = pagedData.TotalCount,
                Data = pagedData.Data.Adapt<List<SetupUserDto>>() // Mapster use kar rahe hain to
            };
            return Ok(result);
        }

        [HttpGet("GetUsersList")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                var lst = await thisService.GetUsersList();
                if(lst == null)
                {
                    return BadRequest(lst);
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var result = await thisService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost("Save")]
        public async Task<ActionResult<int>> Save([FromBody] SetupUserDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data"); // HTTP 400
            }

            var converted = dto.Adapt<SetupUser>();
            var result = await thisService.Save(converted);

            // Agar result 1 hai to Success return karein
            if (result == 1)
                return Ok(result);

            // Agar 0 ya -1 hai to Conflict ya UnprocessableEntity return karein
            if (result == -1)
                return Conflict("User already exists"); // HTTP 409

            return BadRequest("Server error occurred"); // HTTP 400
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(SetupUserDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("User data is required.");

                var converted = dto.Adapt<SetupUser>();
                var result = await thisService.Update(converted);

                // Map your business logic results to HTTP status codes
                if (result == -1)
                    return Conflict("User Name already exists."); // 409 Conflict

                if (result != 1)
                    return BadRequest("Failed to update user.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var result = await thisService.Delete(id);
            if (!result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(long userId, string password)
        {
            if (userId == 0 || password == null)
            {
                return BadRequest("User data and new password are required.");
            }
            
            var result = await thisService.UpdatePassword(userId,password);
            if (!result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetUsersSummary")]
        public async Task<IActionResult> GetUserSummary()
        {
            try
            {
                var response = await thisService.GetUsersSummary();
                if(response == null)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
