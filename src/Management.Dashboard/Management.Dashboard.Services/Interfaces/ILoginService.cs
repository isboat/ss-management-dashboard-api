using Management.Dashboard.Models.Authentication;

namespace Management.Dashboard.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseModel?> Login(LoginModel model);
    }
}
