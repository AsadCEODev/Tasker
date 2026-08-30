using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Services;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssignedTasksController : ControllerBase
    {
        private readonly IUserAssignedTasksService thisService;

        public UserAssignedTasksController(IUserAssignedTasksService service)
        {
            thisService = service;
        }

        [HttpPost("GetTasksByUser")]
        public async Task<IActionResult> GetTasksByUser(FilterModel filter)
        {
            try
            {
                var response = await thisService.GetAllByUser(filter);

                if (response == null || response.Data == null)
                {
                    return BadRequest(new());
                }
                var convertedData = response.Data.Adapt<List<UserTaskDto>>();
                var pagedResult = new PaginationResponse<UserTaskDto>
                {
                    PageIndex = response.PageIndex,
                    PageSize = response.PageSize,
                    TotalCount = response.TotalCount,
                    Data = convertedData
                };

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("UpdateUserTask")]
        public async Task<IActionResult> Update([FromForm] UserTaskDto dto, IFormFile? file)
        {
            try
            {
                var converted = dto.Adapt<UserTask>();
                var resullt = await thisService.Update(converted, file);
                return Ok(resullt);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(SetupTaskDto dto)
        {
            try
            {
                var convertedData = dto.Adapt<SetupTask>();
                var result = await thisService.ChangeStatus(convertedData);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
