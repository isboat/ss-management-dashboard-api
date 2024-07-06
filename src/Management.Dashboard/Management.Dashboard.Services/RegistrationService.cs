using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Models.Authentication;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registerationRepository;

        public RegistrationService(
            IRegistrationRepository userRepository)
        {
            _registerationRepository = userRepository;
        }

        public async Task Register(RegisterModel model)
        {
            if (ValidateRegisterModel(model))
            {
                var dbUser = await _registerationRepository.GetByEmailAsync(model.Email!);
                if (dbUser != null) return;

                await _registerationRepository.CreateAsync(model);
            }
        }

        private static bool ValidateRegisterModel(RegisterModel model)
        {
            if (model == null) return false;
            if (string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.Name)
                || string.IsNullOrEmpty(model.Telephone)
                || string.IsNullOrEmpty(model.Postcode)
                || string.IsNullOrEmpty(model.Address)) return false;
            
            return true;
        }
    }
}
