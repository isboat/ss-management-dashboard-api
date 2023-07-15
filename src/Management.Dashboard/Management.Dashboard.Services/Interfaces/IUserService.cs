using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId);

        Task<UserModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(UserModel newModel);

        public Task UpdateAsync(string id, UserModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
