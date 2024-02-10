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
                throw new Exception("tenant_not_found_in_claims");
            }

            return tenantClaim.Value;
        }

        [NonAction]
        public string GetAuthorizedUserInitials()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("initials", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
                throw new Exception("initials_not_found_in_claims");
            }

            return tenantClaim.Value;
        }

        [NonAction]
        public string GetAuthorizedUserEmail()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
                throw new Exception("email_not_found_in_claims");
            }

            return tenantClaim.Value;
        }
    }
}
