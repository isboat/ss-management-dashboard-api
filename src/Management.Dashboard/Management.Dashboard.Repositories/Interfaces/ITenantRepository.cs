using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface ITenantRepository
    {
        Task<IEnumerable<TenantModel>> GetAllAsync();

        Task<TenantModel?> GetAsync(string id);
    }
}
