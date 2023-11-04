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
    public class DeviceAuthController : CustomBaseController
    {
        private readonly IDeviceAuthService _devicesService;

        public DeviceAuthController(IDeviceAuthService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpPost("device/auth")]
        public async Task<ActionResult> Post([FromBody] DeviceAuthModel deviceModel)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || deviceModel == null)
            {
                return BadRequest();
            }

            deviceModel.TenantId = tenantId;

            await _devicesService.ApproveAsync(deviceModel);
            return NoContent();
        }
    }
}
