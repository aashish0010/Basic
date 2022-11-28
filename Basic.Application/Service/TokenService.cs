using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Basic.Application.Service
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }

        public Tokens GenerateRefreshToken(string userName, string email, string role, string isadmin)
        {

            return GenerateJWTTokens(NormalFunctions.Decrypt(userName), NormalFunctions.Decrypt(email), NormalFunctions.Decrypt(role), NormalFunctions.Decrypt(isadmin));
        }

        public Tokens GenerateToken(string userName, string email, string role, string isadmin)
        {
            return GenerateJWTTokens(userName, email, role, isadmin);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        public Tokens GenerateJWTTokens(string userName, string email, string role, string isadmin)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                 new Claim(ClaimTypes.Name,NormalFunctions.encrypt(userName)),
                 new Claim("Email",NormalFunctions.encrypt(email)),
                 new Claim("Role",NormalFunctions.encrypt(role)),
                 new Claim("Isadmin",NormalFunctions.encrypt(isadmin))
              }),
                Expires = DateTime.Now.AddMinutes(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            return new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken, UserName = userName };
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GetDataFromToken(string token)
        {
            var principal = GetPrincipalFromExpiredToken(token);

            var username = principal.Identity?.Name;

            return username;
        }
        public string GetSpecificTokenData(string token, string value)
        {
            Uservalidate user = new Uservalidate();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == value).Value;
            return NormalFunctions.Decrypt(jti);
        }

        public bool CheckTokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }
        public static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }
    }
}
