using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HomeStorage.Logic.Models.AuthenticationModels;

namespace HomeStorage.API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationLogic _authenticationLogic;

        public AuthenticationController(AuthenticationLogic authenticationLogic)
        {
            _authenticationLogic = authenticationLogic;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {
            TokenModel? token = await _authenticationLogic.LoginAsync(loginModel);

            return token is not null ?
                Ok(token) :
                Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            bool created = await _authenticationLogic.Register(model);
            return created ?
                Ok() : Conflict();
        }
    }
}
