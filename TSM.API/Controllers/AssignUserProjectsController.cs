using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TMS.Shared.Model.Setup;
using TMS.Shared.Pagination;
using TSM.API.Services.SetupServices;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignUserProjectsController : ControllerBase
    {
        private readonly IAssignProjectService thisService;
        public AssignUserProjectsController(IAssignProjectService assignProjectService)
        {
            thisService = assignProjectService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize, string? queryString)
        {
            try
            {
                var result = await thisService.GetAll(pageIndex, pageSize, queryString);
                var converted = result.Adapt<PaginationResponse<UserProjectDto>>();
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

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            try
            {
                var result = await thisService.GetByUserId(userId);
                var converted = result?.Adapt<List<UserProjectDto>>();
                if(converted != null)
                {
                    return Ok(converted);
                }
                return NotFound(converted);
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
                var result = await thisService.GetById(id);
                var converted = result.Adapt<UserProjectDto>();
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
        [HttpPost("Save")]
        public async Task<IActionResult> Save(UserAssignProjectsDto dto)
        {
            try
            {
                var result = await thisService.Save(dto);
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
