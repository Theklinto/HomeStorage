using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.AuthenticationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(AuthenticationLogic authenticationLogic) : Controller
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {
            TokenModel? token = await authenticationLogic.LoginAsync(loginModel);

            return token is not null ?
                Ok(token) :
                Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            bool created = await authenticationLogic.Register(model);
            return created ?
                Ok() : Conflict();
        }
    }
}
