using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApi.Services
{
    public class JWTService(IConfiguration configuration)
    {
        public Object Authenticate(string username, string role)
        {
            var issuer = configuration["JWTConfiguration:Issuer"];
            var audience = configuration["JWTConfiguration:Audience"];
            var key = configuration["JWTConfiguration:Key"];
            var tokenValidityMins = configuration.GetValue<int>("JWTConfiguration:TokenValidityMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Name, username.Trim()),
                    new Claim(ClaimTypes.Role, role.Trim())
                ]),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new
            {
                AccessToken = accessToken,
                Username = username,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
