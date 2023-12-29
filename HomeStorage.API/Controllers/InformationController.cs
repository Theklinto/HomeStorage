using HomeStorage.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace HomeStorage.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/info")]
    public class InformationController : Controller
    {
        [HttpGet]
        [Route("version")]
        public VersionModel GetVersion()
        {
            return new(Assembly.GetExecutingAssembly().GetName().Version?.ToString());
        }
    }
}
