using Management.Dashboard.Common.Constants;
using Management.Dashboard.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    //[Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    [ApiController]
    public class SignalRController : CustomBaseController
    {
        private const string EnableDetailedErrors = "EnableDetailedErrors";

        private readonly ServiceHubContext _messageHubContext;
        private readonly bool _enableDetailedErrors;

        public SignalRController(IHubContextStore store, IConfiguration configuration)
        {
            _messageHubContext = store.MessageHubContext;
            _enableDetailedErrors = configuration.GetValue(EnableDetailedErrors, false);
        }

        [HttpPost("signalr/negotiate")]
        public async Task<ActionResult> HubNegotiate([FromQuery] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest("Device ID is null or empty.");
            }

            var negotiateResponse = await _messageHubContext.NegotiateAsync(new()
            {
                UserId = NotificationExtensions.ToSignalRUserId(deviceId),
                EnableDetailedErrors = _enableDetailedErrors
            });

            return new JsonResult(new Dictionary<string, string>()
            {
                { "url", negotiateResponse.Url! },
                { "accessToken", negotiateResponse.AccessToken! }
            });
        }

        [HttpPost("signalr/add-to-group")]
        public async Task<IActionResult> AddToGroup([FromQuery] string deviceId, [FromQuery]string connectionId)
        {
            var tenantId = "tenantid"; // GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }
             
            var connectionExist = await _messageHubContext.ClientManager.ConnectionExistsAsync(connectionId);
            if(!connectionExist)
            {
                return BadRequest("no_connection_exist");
            }

            var grp = NotificationExtensions.ToGroupName(tenantId);
            var userId = NotificationExtensions.ToSignalRUserId(deviceId);
            if (!await _messageHubContext.UserGroups.IsUserInGroup(userId, grp))
            {
                await _messageHubContext.UserGroups.AddToGroupAsync(userId, grp);
            }

            return NoContent();
        }
    }
}
