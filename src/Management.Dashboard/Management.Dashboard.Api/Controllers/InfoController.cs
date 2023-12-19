using Amazon.Auth.AccessControlPolicy;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public InfoController()
        {
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
