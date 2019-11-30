using System.Reflection;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using DatingAppAPI.Models;
using System.Text;
using System;

namespace DatingAppAPI.Security
{
    public static class TokenUtility
    {
        public static SecurityTokenDescriptor CreateTokenDescriptor(User user, IConfiguration config)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            // Here we grab the token value from AppSettings.json


            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(config.GetSection("AppSettings:Token").Value));

            // Create a new set of signed credentials, based on SHA512 encryption
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create a Token Desciptor for our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
 
            return tokenDescriptor; 
        } 
    }
}