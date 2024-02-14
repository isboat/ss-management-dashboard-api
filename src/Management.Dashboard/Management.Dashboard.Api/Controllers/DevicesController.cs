using Amazon.Auth.AccessControlPolicy;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Notification;
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
        private readonly IMessagePublisher _messagePublisher;

        public DevicesController(IDeviceAuthService devicesService, IMessagePublisher messagePublisher)
        {
            _devicesService = devicesService;
            _messagePublisher = messagePublisher;
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

            await _messagePublisher.SendMessage(new ChangeMessage
            {
                DeviceId = id,
                TenantId = tenantId,
                MessageType = MessageTypes.DeviceInfoUpdate
            });

            return NoContent();
        }


        [HttpPatch("devices/{id}/link-screen")]
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

            await _messagePublisher.SendMessage(new ChangeMessage
            {
                DeviceId = id,
                TenantId = tenantId,
                MessageType = MessageTypes.ContentPublish
            });

            return NoContent();
        }

        [HttpPatch("devices/unlink-screen/{screenId}")]
        public async Task<ActionResult> UnlinkScreenFromDevices(string screenId)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(screenId) || string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var devices = await _devicesService.GetDevicesByScreenId(screenId);

            foreach (var device in devices)
            {
                device.ScreenId = null;
                await _devicesService.UpdateAsync(device.Id!, device);
                await _messagePublisher.SendMessage(new ChangeMessage
                {
                    DeviceId = device.Id!,
                    TenantId = tenantId,
                    MessageType = MessageTypes.ContentPublish
                });
            }

            return NoContent();
        }

        [HttpDelete("devices/{id}")]
        [ProducesResponseType(typeof(DeviceModel), 200)]
        public async Task<IActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _devicesService.DeleteAsync(tenantId, id); 
            await _messagePublisher.SendMessage(new ChangeMessage
            {
                DeviceId = id,
                TenantId = tenantId,
                MessageType = MessageTypes.DeviceInfoUpdate
            });
            return Ok();
        }
    }
}
