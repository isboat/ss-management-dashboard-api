using System.IdentityModel.Tokens.Jwt;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IDictionary<string, string> tokenData, DateTime? expiresOn);

        void ValidateToken(string token);

        JwtSecurityToken ReadToken(string token);
    }
}
