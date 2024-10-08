﻿using Foody.Model.Models;
using Foody.Model.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.JWT
{
    public class JWTService : IJWTService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly JWTData _jWTData;
        private readonly IConfiguration _config;

        public JWTService(UserManager<Customer> userManager, IOptions<JWTData> options, IConfiguration config)
        {
            _userManager = userManager;
            _jWTData = options.Value;
            _config = config;
        }

        public async Task<string> GenerateJwtToken(Customer customer)
        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, customer.Id),
               new Claim(ClaimTypes.Name, customer.UserName),
               new Claim(ClaimTypes.Email, customer.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())    
            };
         
            var roles = await _userManager.GetRolesAsync(customer);

            foreach(var role in roles)
            {
                claims.Add( new Claim(ClaimTypes.Role, role));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30), //+ _jWTData.TokenLifeTime,
                Audience = _jWTData.Issuer,
                Issuer = _jWTData.Issuer,
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
