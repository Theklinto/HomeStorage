using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([FromHeader(Name = "Authorization")] string authHeader)
        {
            ClaimsIdentity? identity = await _authenticationLogic.CookieLoginAsync(authHeader);

            if (identity is null)
                return Unauthorized();

            AuthenticationProperties properties = new()
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), properties);

            return Ok();
        }

        [HttpGet]
        [Route("logout")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            ResponseModel response = await _authenticationLogic.Register(model);
            return response.Success ?
                Ok(response) : Conflict(response);
        }
    }
}
