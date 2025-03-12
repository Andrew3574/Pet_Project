using EventsAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventsAPI.Services
{
    public class AuthenticationService
    {

        public static string GenerateJSONWebToken(Guest userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtHeaderParameterNames.Jku, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Role, userInfo.Role.Name)
            };
            var token = new JwtSecurityToken(
              issuer: "MyAuthServer",
              audience: "MyAuthClient",
              claims: claims,
              expires: DateTime.Now.AddDays(7),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            //_configuration.Build().GetSection("key").Value
            var key = Encoding.ASCII.GetBytes("mysupersecret_secretsecretsecretkey!123");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;/*
                var userName = jwtToken.Claims.First(claim => claim.Type == "jku").Value;
                var role = jwtToken.Claims.First(claim=> claim.Type == ClaimTypes.Role).Value;*/
                var identity = new ClaimsIdentity(jwtToken.Claims.ToList(), "jwt");
                var principals = new ClaimsPrincipal(identity);
                return principals;
            }
            catch
            {
                return null;
            }
        }

    }
}
