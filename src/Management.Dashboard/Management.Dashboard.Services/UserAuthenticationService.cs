using Management.Dashboard.Models;
using Management.Dashboard.Models.Authentication;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Dashboard.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository<UserModel> _userRepository;

        public UserAuthenticationService(IJwtService jwtService, IUserRepository<UserModel> userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<LoginResponseModel?> Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                return null;
            }

            var dbUser = await _userRepository.GetByEmailPasswordAsync(model.Username, model.Password!);
            if (dbUser == null) return null;

            var tokenData = new Dictionary<string, string>
            {
                { "tenantid", dbUser.TenantId! },
                { "email", dbUser.Email! },
            };
            var tokenResponse = new LoginResponseModel
            {
                Token = _jwtService.GenerateToken(tokenData, DateTime.UtcNow.AddHours(2))
            };

            return tokenResponse;
        }
    }
}
