using Amazon.Auth.AccessControlPolicy;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class ScreenController : CustomBaseController
    {
        private readonly IScreenService _screenService;

        public ScreenController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet("screens")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _screenService.GetScreensAsync(tenantId);
            if (data == null)
            {
                return NotFound();
            }

            return new JsonResult(data);
        }

        [HttpGet("screens/{id}")]
        [ProducesResponseType(typeof(ScreenModel), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _screenService.GetAsync(tenantId, id);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpPost("screens")]
        public async Task<ActionResult> Post([FromBody] ScreenModel screenModel)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || screenModel == null)
            {
                return BadRequest();
            }

            screenModel.TenantId = tenantId;

            await _screenService.CreateAsync(screenModel);
            return NoContent();
        }

        [HttpPatch("screens")]
        public async Task<ActionResult> Patch([FromBody] ScreenModel screenModel)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(screenModel?.Id) || string.IsNullOrEmpty(screenModel?.TenantId))
            {
                return BadRequest();
            }

            await _screenService.UpdateAsync(screenModel.Id, screenModel);
            return NoContent();
        }

        [HttpDelete("screens/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _screenService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
