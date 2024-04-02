//using codepulse.API.Repositories.Interface;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Server.IISIntegration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace codepulse.API.Repositories.Implementation
//{
//    public class TokenRepo : ITokenRepo

//    {

//        private readonly IConfiguration configuration;
//        public TokenRepo(IConfiguration configuration)

//        {
//          this.configuration = configuration;
//        }


//        public string CreateJwtToken(IdentityUser user, List<string> roles)
//        {
//            //Create Claims
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Email, user.Email),
//            };

//            claims.AddRange(roles.Select(role=> new Claim (ClaimTypes.Role, role)));



//            //Jwt Security Token Parameters

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: configuration["Jwt:Issuer"],
//                audience: configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(15),
//                signingCredentials: credentials);

               
//            // Return token
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}



using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace codepulse.API.Repositories.Implementation
{
    public class TokenRepo : ITokenRepo
    {
        private readonly IConfiguration _configuration;

        public TokenRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Create Claims
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Jwt Security Token Parameters
            var key = GenerateHMACSHA256Key();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // Return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SymmetricSecurityKey GenerateHMACSHA256Key()
        {
            using var hmac = new HMACSHA256();
            return new SymmetricSecurityKey(hmac.Key);
        }
    }
}

