using Management.Dashboard.Models.Authentication;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task Register(RegisterModel model);
    }
}
