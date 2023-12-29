using HomeStorage.Logic.Models.AuthenticationModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        //public async Task<ResponseModel> Register(RegisterModel model)
        //{
        //    IdentityUser? user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user is not null)
        //        return new()
        //        {
        //            Message = "Email er allerede i brug af en anden bruger."
        //        };

        //    user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username,
        //    };
        //    IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded is false)
        //        return new()
        //        {
        //            Message = ("Kunne ikke oprette brugeren, tjek de angivne oplysninger og prøv igen.\r\n" +
        //                string.Join("\r\n", result.Errors.Select(x => x.Description))).Trim()
        //        };

        //    return new()
        //    {
        //        Success = true,
        //        Message = "Brugeren blev oprettet korrekt."
        //    };
        //}

        public async Task<TokenModel?> LoginAsync(LoginModel loginModel)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user is null)
                return null;

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            };

            DateTime expiration = DateTime.Now.AddDays(_configuration.GetValue<int>("JWTSettings:Expiration"));

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSettings:Secret")!));
            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                _configuration["JWTSettings:Issuer"],
                _configuration["JWTSettings:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signIn);

            return new()
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
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
