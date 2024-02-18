using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            // We used this interface because we are going to pull stuff
            // from the appsettings.json file
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser user)
        {
            // Create claims
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            // Create credentials
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor{
                // ClaimsIdentity acts as a container for a collection of claims
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]

            };

            // Configure the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create an object representation of an object
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token in textual format
            return tokenHandler.WriteToken(token);
        }
    }

    internal class SymmertricSecurityKey
    {
    }
}