using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Management.Dashboard.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtSigningKey;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _jwtSecurityTokenHandler = new();

            _jwtIssuer = settings.Value.Issuer;
            _jwtAudience = settings.Value.Audience;
            _jwtSigningKey = settings.Value.SigningKey;
        }

        public string GenerateToken(IDictionary<string, string> tokenData, DateTime? expiresOn)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSigningKey));

            var claims = tokenData.Select(x => new Claim(x.Key, x.Value)).ToArray();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresOn ?? DateTime.UtcNow.AddHours(1),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public void ValidateToken(string token)
        {
            var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,

                    // Allow for some drift in server time
                    // (a lower value is better; we recommend two minutes or less)
                    ClockSkew = TimeSpan.FromSeconds(0),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSigningKey))
                }, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
        }

        public JwtSecurityToken ReadToken(string token)
        {
            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            return jwtToken;
        }
    }
}
