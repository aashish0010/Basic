﻿using Basic.Application.Function;
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
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key)

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

            var audiance = _configuration["JWT:Audience"];
            var issuer = _configuration["JWT:Issuer"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
              {
                  new Claim(ClaimTypes.Name, NormalFunctions.encrypt(userName)),
                  new Claim("Email", NormalFunctions.encrypt(email)),
                  new Claim("Role", NormalFunctions.encrypt(role)),
                  new Claim("Isadmin", NormalFunctions.encrypt(isadmin))
              }),

                Audience = audiance,
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            return new Tokens
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                UserName = userName
            };
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

        public bool CheckTheTokenTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);

            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }


        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
        public bool IsTokenValid(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var mySecurityKey = new SymmetricSecurityKey(Key);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:Key"].ToString(),
                    ValidAudience = _configuration["JWT:Audience"].ToString(),
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    LifetimeValidator = CustomLifetimeValidator

                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }


    }
}
