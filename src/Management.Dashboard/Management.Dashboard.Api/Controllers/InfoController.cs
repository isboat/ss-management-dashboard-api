using Management.Dashboard.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Net.Sockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// https://isboatservices.grafana.net/explore?schemaVersion=1&panes=%7B%22zxd%22:%7B%22datasource%22:%22grafanacloud-logs%22,%22queries%22:%5B%7B%22refId%22:%22A%22,%22expr%22:%22%7Bapp%3D%5C%22web_app%5C%22%7D%22,%22queryType%22:%22range%22,%22datasource%22:%7B%22type%22:%22loki%22,%22uid%22:%22grafanacloud-logs%22%7D,%22editorMode%22:%22builder%22%7D%5D,%22range%22:%7B%22from%22:%22now-1h%22,%22to%22:%22now%22%7D%7D%7D&orgId=1
namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class InfoController : CustomBaseController
    {
        private readonly IMessagePublisher broadcastService;
        private readonly ILogger<InfoController> _logger;

        public InfoController(IMessagePublisher broadcastService, ILogger<InfoController> logger)
        {
            this.broadcastService = broadcastService;
            this._logger = logger;
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
            _logger.LogError("some error message");
            return new OkObjectResult(new { success = "true" });
        }

        [HttpGet("messages")]
        [ProducesResponseType(200)]
        public IActionResult SignalRMessageTest(string info, string mType)
        {
            if (string.IsNullOrEmpty(info))
            {
                this.broadcastService.SendMessage(new ChangeMessage 
                { 
                    MessageType = MessageTypes.ContentPublish, 
                    DeviceId = "61ac1c5dc5d64c57ac8fdc50d1ea2f32", 
                    TenantId = "onscreensync_testing_ltd_tenant" 
                });
            }
            else
            {
                this.broadcastService.SendMessage(new ChangeMessage
                {
                    MessageType = mType,
                    MessageData = info
                });
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
