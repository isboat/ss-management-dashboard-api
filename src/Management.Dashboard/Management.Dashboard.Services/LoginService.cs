using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Models.Authentication;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class LoginService : ILoginService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public LoginService(
            IJwtService jwtService, 
            IUserRepository<UserModel> userRepository, 
            IEncryptionService encryptionService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<LoginResponseModel?> Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return null;
            }

            var dbUser = await _userRepository.GetByEmailAsync(model.Username);
            if (string.IsNullOrEmpty(dbUser?.Password)) return null;

            var verified = _encryptionService.Verify(model.Password, dbUser.Password);
            if(!verified) return null;

            var tokenData = new Dictionary<string, string>
            {
                { "tenantid", dbUser.TenantId! },
                { "email", dbUser.Email! },
                { "initials", GetInitials(dbUser)! },
                { "scope", TenantAuthorization.RequiredScope },
            };
            if (dbUser.Role.HasValue)
            {
                tokenData.Add("role", dbUser.Role.Value.ToString());
            }
            var tokenResponse = new LoginResponseModel
            {
                Token = _jwtService.GenerateToken(tokenData, DateTime.UtcNow.AddHours(100))
            };

            return tokenResponse;
        }
        private static string GetInitials(UserModel dbUser)
        {
            if(string.IsNullOrEmpty(dbUser?.Name)) return string.Empty;

            var splits = dbUser.Name.Split(" ");
            if (splits.Length == 0) return string.Empty;

            var ss = "";
            foreach (var item in splits)
            {
                ss += item[0];
            }

            return ss.ToUpperInvariant();
        }
    }
}
