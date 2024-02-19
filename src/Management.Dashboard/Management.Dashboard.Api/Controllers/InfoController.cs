using Management.Dashboard.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Net.Sockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
