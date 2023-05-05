using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LocationLogic _locationLogin;
        public LocationController(UserManager<IdentityUser> userManager, LocationLogic locationLogic)
        {
            _userManager = userManager;
            _locationLogin = locationLogic;
        }

        [HttpPost("CreateLocation")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateLocation(LocationModel model)
        {
            IdentityUser? user = await this.GetCurrentUser(_userManager);
            if (user is null)
                return Unauthorized();

            model = await _locationLogin.CreateLocationAsync(model, user);
            return Created(Request.GetEncodedUrl(), model);
        }

        [HttpGet("GetLocation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocation([FromQuery]Guid locationId)
        {
            IdentityUser? user = await this.GetCurrentUser(_userManager);
            if(user is null)
                return Unauthorized();
            
            LocationModel? model = await _locationLogin.GetLocationAsync(locationId, user);
            if(model is null)
                return NoContent();

            return Ok(model);
        }

        [HttpGet("GetAllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLocations()
        {
            IdentityUser? user = await this.GetCurrentUser(_userManager);
            if (user is null)
                return Unauthorized();

            IEnumerable<LocationModel> locations = _locationLogin.GetAllLocationsByUser(user);
            return Ok(locations);
        }
    }
}
