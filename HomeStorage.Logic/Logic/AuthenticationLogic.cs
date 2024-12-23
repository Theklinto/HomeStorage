using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Authentication;
using HomeStorage.Logic.Models.AuthenticationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeStorage.Logic.Logic
{
    public class AuthenticationLogic(HomeStorageUserManager userManager, IConfiguration config, HomeStorageSignInManager signInManager)
    {
        private readonly HomeStorageUserManager _userManager = userManager;
        private readonly IConfiguration _config = config;
        private readonly HomeStorageSignInManager _signInManager = signInManager;

        public async Task<bool> Register(RegisterModel model)
        {

            HomeStorageUser newUser = new()
            {
                Email = model.Email,
                UserName = model.Username,
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded is false)
                return false;

            return true;
        }

        public async Task<TokenModel?> LoginAsync(LoginModel loginModel)
        {

            HomeStorageUser? user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user is null)
                return null;

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!result.Succeeded)
                return null;



            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
            ];

            DateTime expiration = DateTime.Now.AddDays(_config.GetValue<int>("JWTSettings:Expiration"));

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWTSettings:Secret")!));
            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                _config["JWTSettings:Issuer"],
                _config["JWTSettings:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signIn);

            return new()
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
