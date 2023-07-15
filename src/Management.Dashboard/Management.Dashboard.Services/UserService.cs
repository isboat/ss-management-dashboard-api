using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<UserModel> _repository;

        public UserService(IUserRepository<UserModel> userRepository)
        {
            _repository = userRepository;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<UserModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task CreateAsync(UserModel newModel)
        {
            AddId(newModel);
            newModel.Created = DateTime.UtcNow;
            await _repository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}