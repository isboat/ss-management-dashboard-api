using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreenController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet("{tenantId}/screens")]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<ScreenModel>> Get(string tenantId)
        {
            return await _screenService.GetScreensAsync(tenantId);
        }

        [HttpGet("{tenantId}/screens/{id}")]
        [ProducesResponseType(typeof(ScreenModel), 200)]
        public async Task<ScreenModel?> Get(string tenantId, string id)
        {
            return await _screenService.GetAsync(tenantId, id);
        }

        [HttpPost("{tenantId}/screens")]
        public async Task<ActionResult> Post(string tenantId, [FromBody] ScreenModel screenModel)
        {
            if (string.IsNullOrEmpty(tenantId) || screenModel == null)
            {
                return BadRequest();
            }

            await _screenService.CreateAsync(screenModel);
            return NoContent();
        }

        [HttpPatch("{tenantId}/screens")]
        public async Task<ActionResult> Patch(string tenantId, [FromBody] ScreenModel screenModel)
        {
            if (string.IsNullOrEmpty(tenantId) || screenModel == null)
            {
                return BadRequest();
            }

            await _screenService.CreateAsync(screenModel);
            return NoContent();
        }

        [HttpDelete("{tenantId}/screens/{id}")]
        public async Task<ActionResult> Delete(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _screenService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
