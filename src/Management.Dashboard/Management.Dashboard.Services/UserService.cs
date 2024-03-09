using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;
using System.Text;

namespace Management.Dashboard.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<UserModel> _repository;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailSender _emailSender;

        public UserService(
            IUserRepository<UserModel> userRepository, 
            IEncryptionService encryptionService, 
            IEmailSender emailSender)
        {
            _repository = userRepository;
            _encryptionService = encryptionService;
            _emailSender = emailSender;
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
            await _emailSender.SendEmailAsync(newModel.Email!, "onScreenSync user account created", GetUserCreatedEmailbody(newModel));
        }

        private static string GetUserCreatedEmailbody(UserModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Dear {model.Name},</p>");
            builder.Append($"<p>Welcome to onScreenSync TV Screen Management service! Welcome onboard as a content editor</p>");
            builder.Append("<ul>");
            builder.Append($"<li>Take some time to navigate through our platform and discover all the tools and features we offer to help you. Visit <a href='http://myscreensyncservice.runasp.net/'>Management Dashboard</a> to get started</li>");
            builder.Append("</ul>");
            builder.Append("<p>If you have any questions or need assistance, don't hesitate to reach out to our support team at support@onscreensync.com or visit our Help Center for <a href='https://onscreensync.com/faq.html'>FAQs and troubleshooting guides</a>.</p>");
            builder.Append("<p>Best regards,<br />onScreenSync.com<p/>");
            return builder.ToString();
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