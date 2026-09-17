using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TMS.Shared.Enum;
using TMS.Shared.Model;
using TMS.Shared.Model.Filters;
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

        [HttpGet("GetRolesList")]
        public async Task<IActionResult> GetRolesList()
        {
            try
            {
                var data = await thisService.GetRolesList();
                if (data == null)
                {
                    return NotFound(new());
                }
                var converted = data.Adapt<List<AppRoleDto>>();
                return Ok(converted);
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
                var data = await thisService.GetAllAsync( filters);
                if(data == null)
                {
                    return NotFound(new());
                }
                var converted = data.Adapt<List<AppRoleDto>>();
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
                var data = await thisService.GetById(id);
                if (data == null)
                {
                    return NotFound(new());
                }
                var converted = data.Adapt<AppRoleDto>();
                return Ok(converted);
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await thisService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
