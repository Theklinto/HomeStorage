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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            JwtTokenModel jwtToken = await _authenticationLogic.Login(model);
            if (jwtToken.Expiration.GetValueOrDefault() > DateTime.Now)
                return Ok(jwtToken);

            return Unauthorized();

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            ResponseModel response = await _authenticationLogic.Register(model);
            return response.Success ?
                Ok(response) : Conflict(response);
        }
    }
}
