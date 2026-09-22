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
    public class AppUserRoleController : ControllerBase
    {
        private readonly IAppUserRoleService thisService;

        public AppUserRoleController(IAppUserRoleService appUserRoleService)
        {
            thisService = appUserRoleService;
        }

        [HttpGet("GetUserPermissionsList")]
        public async Task<IActionResult> GetUserPermissionsList()
        {
            try
            {
                var lst = await thisService.GetUserPermissionList();
                if(lst == null)
                {
                    return NotFound(new());
                }
                var converted = lst.Adapt<List<AppUserRoleDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        [HttpPost("GetUnAssignedUsersList")]
        public async Task<IActionResult> GetUnAssignedUsersList(FilterModel filter)
        {
            try
            {
                var lst = await thisService.GetUnAssignedUsersList( filter);
                if(lst == null)
                {
                    return NotFound(new());
                }
                var converted = lst.Adapt<List<AppUsersListDto>>();
                return Ok(converted);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("GetAssignedUsersList")]
        public async Task<IActionResult> GetAssignedUsersList(FilterModel filter)
        {
            try
            {
                var data = await thisService.GetSelelctedUsers(filter);
                if(data == null)
                {
                    return NotFound(new());
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(FilterModel filters)
        {
            try
            {
                var data = await thisService.GetAll(filters);

                if (data == null || data.Data == null || !data.Data.Any())
                {
                    return NotFound(new PaginationResponse<AppUserRoleDto>());
                }

                var mappedList = data.Data.Adapt<List<AppUserRoleDto>>();

                var response = new PaginationResponse<AppUserRoleDto>
                {
                    Data = mappedList,
                    TotalCount = data.TotalCount,
                    PageIndex = data.PageIndex,
                    PageSize = data.PageSize
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate( AppUserRoleDto dto)
        {
            try
            {
                var converted = dto.Adapt<AppUserRole>();
                var result = await thisService.SaveOrUpdateUsersRoleAsync(converted);
                if (result) return Ok(result);
                else return BadRequest(result);
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
                if(result) 
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
