using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TSM.API.Services.SetupServices;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService thisService;

        public TagsController(ITagsService service)
        {
            try
            {
                thisService = service;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await thisService.GetAll();
                if(result == null)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await thisService.GetById(id);
                if (result == null)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
