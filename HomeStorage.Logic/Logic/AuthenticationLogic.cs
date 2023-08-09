using Azure;
using HomeStorage.DataAccess.AuthenticationModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Logic
{
    public class AuthenticationLogic
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;


        public AuthenticationLogic(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ResponseModel> Register(RegisterModel model)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
                return new()
                {
                    Message = "Email er allerede i brug af en anden bruger."
                };

            user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false)
                return new()
                {
                    Message = ("Kunne ikke oprette brugeren, tjek de angivne oplysninger og prøv igen.\r\n" +
                        string.Join("\r\n", result.Errors.Select(x => x.Description))).Trim()
                };

            return new()
            {
                Success = true,
                Message = "Brugeren blev oprettet korrekt."
            };
        }

        public async Task<ClaimsIdentity?> CookieLoginAsync(string authHeader)
        {
            // Get credentials
            string encodedEmailPassword = authHeader[("Basic ".Length - 1)..].Trim();
            string emailPassword = Encoding
            .GetEncoding("iso-8859-1")
            .GetString(Convert.FromBase64String(encodedEmailPassword));

            // Get email and password
            int seperatorIndex = emailPassword.IndexOf(':');
            string email = emailPassword[..seperatorIndex];
            string password = emailPassword[(seperatorIndex + 1)..];

            IdentityUser? user = await _userManager.FindByEmailAsync(email);
            if (user is not null && await _userManager.CheckPasswordAsync(user, password))
            {
                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return identity;
            }

            return null;
        }
    }
}
