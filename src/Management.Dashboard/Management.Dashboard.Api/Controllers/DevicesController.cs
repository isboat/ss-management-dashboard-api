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
    public class DevicesController : CustomBaseController
    {
        private readonly IDevicesService _devicesService;

        public DevicesController(IDevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpGet("devices")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _devicesService.GetDevicesAsync(tenantId);
            if (data == null)
            {
                return NotFound();
            }

            return new JsonResult(data);
        }

        [HttpGet("devices/{id}")]
        [ProducesResponseType(typeof(DeviceModel), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _devicesService.GetAsync(tenantId, id);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpPost("devices")]
        public async Task<ActionResult> Post([FromBody] DeviceModel deviceModel)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || deviceModel == null)
            {
                return BadRequest();
            }

            deviceModel.TenantId = tenantId;

            await _devicesService.CreateAsync(deviceModel);
            return NoContent();
        }

        [HttpPatch("devices")]
        public async Task<ActionResult> Patch([FromBody] DeviceModel deviceModel)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(deviceModel?.Id) || string.IsNullOrEmpty(deviceModel?.TenantId))
            {
                return BadRequest();
            }

            await _devicesService.UpdateAsync(deviceModel.Id, deviceModel);
            return NoContent();
        }

        [HttpDelete("devices/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _devicesService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
