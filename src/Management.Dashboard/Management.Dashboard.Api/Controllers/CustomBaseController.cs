using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    public class CustomBaseController : ControllerBase
    {

        [NonAction]
        public string GetRequestTenantId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("tenantid", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
            }

            return tenantClaim.Value;
        }
    }
}
