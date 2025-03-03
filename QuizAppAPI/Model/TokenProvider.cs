using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Model
{
    public class TokenProvider(IConfiguration configuration)
    {
        public string Create(User user)
        {
            string secretKey = configuration["Jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        //new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        //new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, user.UserName)
                        //new Claim(JwtRegisteredClaimNames.Profile, user.Role)

                    ]),
                    Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:Expiration")),
                    SigningCredentials = credentials,
                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"]
            };
            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDesc);
            return token;
        }
    }
}
