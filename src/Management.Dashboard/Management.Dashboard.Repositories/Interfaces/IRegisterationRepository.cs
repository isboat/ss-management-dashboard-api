using Management.Dashboard.Models.Authentication;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IRegisterationRepository : IRepository<RegisterModel>
    {
        Task<RegisterModel?> GetByEmailAsync(string email);
    }
}
