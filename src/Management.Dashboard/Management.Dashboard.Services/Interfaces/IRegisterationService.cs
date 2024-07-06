using Management.Dashboard.Models.Authentication;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IRegisterationService
    {
        Task Register(RegisterModel model);
    }
}
