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
        private readonly IDeviceAuthService _devicesService;

        public DevicesController(IDeviceAuthService devicesService)
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

            var data = await _devicesService.GetApprovedDevicesAsync(tenantId);
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


        [HttpPatch("devices/{id}/name")]
        public async Task<ActionResult> PatchName(string id, [FromBody] DeviceUpdateRequestModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id))
            {
                return BadRequest();
            }

            await _devicesService.UpdateAsync(model.Id, new DeviceAuthModel 
            { 
                Id = model.Id, 
                DeviceName = model.DeviceName, 
                TenantId = tenantId
            });

            return NoContent();
        }


        [HttpPatch("devices/{id}/screen")]
        public async Task<ActionResult> Patch(string id, [FromBody] DeviceUpdateRequestModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id))
            {
                return BadRequest();
            }

            await _devicesService.UpdateAsync(model.Id, new DeviceAuthModel
            {
                Id = model.Id,
                ScreenId = model.ScreenId,
                TenantId = tenantId
            });

            return NoContent();
        }
    }
}
