using Management.Dashboard.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using System.Net.NetworkInformation;
using System.Net.Sockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class InfoController : CustomBaseController
    {
        private readonly IBroadcastService broadcastService;

        public InfoController(IBroadcastService broadcastService)
        {
            this.broadcastService = broadcastService;
        }

        [HttpGet("getaddress")]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            var ipaddress = GetLocalIPv4(NetworkInterfaceType.Ethernet);
            var wireless = GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            return new OkObjectResult(new { ipadress = ipaddress, wirelessAdd = wireless });
        }

        [HttpGet("health")]
        [ProducesResponseType(200)]
        public IActionResult GetHealth()
        {

            return new OkObjectResult(new { success = "true" });
        }

        [HttpGet("messages")]
        [ProducesResponseType(200)]
        public IActionResult GetHealggth()
        {
            for (int i = 0; i < 1; i++)
            {
                //this.broadcastService.TryBroadcastAsync(new ChangeMessage { DeviceId = "ddd", TenantId = "tenantid" });
            }

            return Ok();
        }

        private string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
}
