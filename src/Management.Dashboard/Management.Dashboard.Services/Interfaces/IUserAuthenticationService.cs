using Management.Dashboard.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<LoginResponseModel?> Login(LoginModel model);
    }
}
