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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService thisService;

        public TasksController(ITaskService service)
        {
            thisService = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var lst = await thisService.GetAll(pageNumber,pageSize);
                if(lst == null)
                {
                    return NotFound(lst);
                }
                var converted = lst.Adapt<PaginationResponse<SetupTaskDto>>();
                return Ok(converted);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Save")]
        public async Task<ActionResult<int>> Save(SetupTaskDto dto)
        {
            try
            {
                var converted = dto.Adapt<SetupTask>();
                int result = await thisService.Save(converted);
                if (result == 1)
                    return Ok(result);

                if (result == -1)
                    return Conflict("Task already exists");

                return BadRequest("Task Not Saved Successfully."); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult<int>> Update(SetupTaskDto dto)
        {
            try
            {
                var converted = dto.Adapt<SetupTask>();
                int result = await thisService.Update(converted);
                if (result == 1)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }
    }
}
