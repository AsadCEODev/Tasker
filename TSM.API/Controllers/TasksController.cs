using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.Services.SetupServices;
using TMS.Shared.Model.Filters;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService thisService;

        public TasksController(ITaskService service)
        {
            thisService = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(FilterDto filter)
        {
            try
            {
                var converteddto = filter.Adapt<FilterModel>();
                var lst = await thisService.GetAll(converteddto);
                if (lst == null)
                {
                    return NotFound(lst);
                }
                var converted = lst.Adapt<PaginationResponse<SetupTaskDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var found = await thisService.GetById(id);
                if (found == null)
                {
                    return NotFound(found);
                }
                var converted = found.Adapt<SetupTaskDto>();
                return Ok(converted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Save")]
        public async Task<ActionResult<int>> Save([FromForm] SetupTaskDto dto,IFormFile? file)
        {
            try
            {
                var converted = dto.Adapt<SetupTask>();

                int result = await thisService.Save(converted, file);

                if (result == 1)
                    return Ok(result);

                if (result == -1)
                    return Conflict("Task already exists");

                return BadRequest("Task Not Saved Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult<int>> Update([FromForm] SetupTaskDto dto, IFormFile? file)
        {
            try
            {
                var converted = dto.Adapt<SetupTask>();

                // Service mein file pass ki ja rahi hai
                int result = await thisService.Update(converted, file);

                if (result == 1)
                    return Ok(result);

                if (result == -1)
                    return Conflict("Task already exists");

                return BadRequest("Task Not Updated Successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await thisService.Delete(id);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("GetTasksSummary")]
        public async Task<IActionResult> GetTasksSummary(FilterDto filter)
        {
            try
            {
                var converted = filter.Adapt<FilterModel>();
                var result = await thisService.GetTasksSummary(converted);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}