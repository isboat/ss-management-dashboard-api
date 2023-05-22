using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models.Authentication;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [EnableCors(TenantAuthorization.RequiredCorsPolicy)]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        // POST api/<Login>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userAuthenticationService.Login(model);
                if(response != null)
                {
                    return new OkObjectResult(response);
                }

                return Unauthorized();
            }

            return BadRequest();
        }
    }
}
