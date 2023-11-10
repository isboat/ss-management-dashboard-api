using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class DeviceAuthController : CustomBaseController
    {
        private readonly IDeviceAuthService _devicesService;

        public DeviceAuthController(IDeviceAuthService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpPost("device/auth")]
        public async Task<ActionResult> Post([FromBody] DeviceAuthRequestModel deviceAuthRequest)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(deviceAuthRequest.UserCode))
            {
                return BadRequest();
            }

            var model = new DeviceAuthModel
            {
                UserCode = deviceAuthRequest.UserCode.ToUpperInvariant(),
                TenantId = tenantId,
            };
            var result = await _devicesService.ApproveAsync(model);

            return result switch
            {
                DeviceAuthApprovalStatus.Success => NoContent(),
                DeviceAuthApprovalStatus.Failed => BadRequest(),
                DeviceAuthApprovalStatus.NotFound => NotFound(),
                DeviceAuthApprovalStatus.BadRequest => BadRequest(),
                DeviceAuthApprovalStatus.TenantNotFound => BadRequest(),
                DeviceAuthApprovalStatus.DeviceLimitReached => BadRequest("device_limit_reached"),
                DeviceAuthApprovalStatus.AlreadyApproved => BadRequest("already_approved"),
                _ => BadRequest(),
            };
        }
    }
}
