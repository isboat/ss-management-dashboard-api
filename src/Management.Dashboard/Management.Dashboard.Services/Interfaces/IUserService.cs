using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId, int? skip, int? limit);

        Task<UserModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(UserModel newModel);

        public Task UpdateAsync(string id, UserModel updatedModel);

        public Task<UpdatePasswordResult> UpdatePasswordAsync(string tenantId, string id, string currentPasswd, string newPasswd);
        public Task<UpdatePasswordResult> ResetPasswordAsync(string tenantId, string id);

        public Task RemoveAsync(string tenantId, string id);
    }
}
