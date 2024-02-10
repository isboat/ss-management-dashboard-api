using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<UserModel> _repository;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository<UserModel> userRepository, IEncryptionService encryptionService)
        {
            _repository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId, int? skip, int? limit)
        { 
            var dbusers = await _repository.GetAllByTenantIdAsync(tenantId, skip, limit);
            if (dbusers == null) return null!;

            dbusers.ForEach(x => x.Password = null);
            return dbusers;
        }

        public async Task<UserModel?> GetAsync(string tenantId, string id)
        {
            var user = await _repository.GetAsync(tenantId, id);
            if (user != null) user.Password = null;

            return user;
        }            

        public async Task CreateAsync(UserModel newModel)
        {
            if (await UserExist(newModel.Email)) throw new Exception("user_with_same_email_exist");

            AddId(newModel);
            newModel.Password = _encryptionService.Encrypt("Temporary!")?.Hashed;
            
            await _repository.CreateAsync(newModel);
        }

        private async Task<bool> UserExist(string? email)
        {
            var user = await _repository.GetByEmailAsync(email!);
            return user != null;
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
           // updatedModel.Password = _encryptionService.Encrypt(updatedModel.Password!)?.Hashed;
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task<UpdatePasswordResult> UpdatePasswordAsync(string tenantId, string id, string currentPasswd, string newPasswd)
        {
            var result = new UpdatePasswordResult();

            var dbUser = await _repository.GetAsync(tenantId, id);
            if (string.IsNullOrEmpty(dbUser?.Password))
            {
                result.Error = "user_not_found";
                return result;
            }

            var verified = _encryptionService.Verify(currentPasswd, dbUser.Password);
            if (!verified)
            {
                result.Error = "current_password_authentication_failed";
                return result;
            };

            await _repository.UpdatePasswordAsync(tenantId, id, _encryptionService.Encrypt(newPasswd!)?.Hashed!);
            result.Success = true;
            return result;
        }

        public async Task<UpdatePasswordResult> ResetPasswordAsync(string tenantId, string id)
        {
            var result = new UpdatePasswordResult();

            var dbUser = await _repository.GetAsync(tenantId, id);
            if (string.IsNullOrEmpty(dbUser?.Password))
            {
                result.Error = "user_not_found";
                return result;
            }

            await _repository.UpdatePasswordAsync(tenantId, id, _encryptionService.Encrypt("Temporary!")?.Hashed!);
            result.Success = true;
            return result;
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}