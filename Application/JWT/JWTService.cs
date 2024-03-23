using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.JWT
{
    public class JWTService
    {

        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User model)
        {

            List<Claim> _claims =
                [
                   new Claim("Id",model.Id.ToString()),
                   new Claim("Email",model.Email),
                   new Claim("LastName",model.LastName),
                   new Claim("Name",model.Name),
                   new Claim("SurName",model.SurName),
                   new Claim("Role",model.Role),
                ];


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));


            var signToken = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                claims: _claims,
                issuer: "localhost",
                audience: "localhost",
                expires: DateTime.Now.AddHours(12),
                signingCredentials: signToken
                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public User DecodeToken(string? token = null)
        {

            var userInfo = new User();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value);
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "localhost",
                    ValidIssuer = "localhost"
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                var jwtSecurityToken = validatedToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }


                var payload = principal.Claims;
                foreach (var claim in payload)
                {
                    switch (claim.Type)
                    {
                        case "Id":
                            if (int.TryParse(claim.Value, out var id))
                            {
                                userInfo.Id = id;
                            }
                            break;
                        case "Email":
                            userInfo.Email = claim.Value;
                            break;
                        case "LastName":
                            userInfo.LastName = claim.Value;
                            break;
                        case "Name":
                            userInfo.Name = claim.Value;
                            break;
                        case "SurName":
                            userInfo.SurName = claim.Value;
                            break;
                        case "Role":
                            userInfo.Role = claim.Value;
                            break;

                    }

                }

                return userInfo;

            }
            catch (Exception ex)
            {

            }

            return userInfo;
            



        }

    }
}
