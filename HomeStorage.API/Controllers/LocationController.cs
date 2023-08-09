using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HomeStorage.API.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LocationLogic _locationLogic;
        public LocationController(UserManager<IdentityUser> userManager, LocationLogic locationLogic)
        {
            _userManager = userManager;
            _locationLogic = locationLogic;
        }

        [HttpPost("CreateLocation")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateLocation([FromForm] LocationUpdateModel model)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            LocationModel result = await _locationLogic.CreateLocationAsync(model, user);
            return Created(Request.GetEncodedUrl(), Json(result).Value);
        }

        [HttpGet("GetLocation/{locationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocation([FromRoute] Guid locationId)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            LocationModel? model = await _locationLogic.GetLocationAsync(locationId, user);
            if (model is null)
                return NoContent();

            return Ok(Json(model).Value);
        }

        [HttpGet("GetAllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLocations()
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            IEnumerable<LocationModel> locations = await _locationLogic.GetAllLocationsByUser(user);
            return Ok(Json(locations).Value);
        }

        [HttpGet("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetListData()
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            IEnumerable<LocationListModel> locations = await _locationLogic.GetList(user);
            return Ok(Json(locations).Value);
        }
        [HttpPut("UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateLocation([FromForm]LocationUpdateModel model)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            LocationModel? updatedLocation = await _locationLogic.UpdateLocation(model, user);

            return updatedLocation is null ?
                BadRequest() :
                Ok(Json(updatedLocation).Value);
        }

        [HttpDelete("DeleteLocation/{locationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLocation([FromRoute] Guid locationId)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            (LocationModel? model, bool? allowDeletion) = await _locationLogic.DeleteLocation(locationId, user);

            if (allowDeletion is false)
                return Unauthorized("The user is not an admin or owner of the location");

            if (model is null)
                return BadRequest("No location found");

            return Ok(Json(model).Value);
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LocationUserModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLocationUsers([FromQuery] Guid locationId)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            List<LocationUserModel>? locationUsers = await _locationLogic.GetLocationUsersAsync(locationId, user);

            if(locationUsers is null)   
                return BadRequest();

            return Ok(Json(locationUsers).Value);
        }

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationUserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLocationUser([FromForm] LocationUserModel model)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            LocationUserModel? locationUser = await _locationLogic.AddUserToLocation(model, user);

            if (locationUser is null)
                return BadRequest();

            return Ok(Json(locationUser).Value);
        }

        [HttpDelete("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationUserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLocationUser([FromQuery] Guid locationUserId)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            LocationUserModel? locationUser = await _locationLogic.DeleteUserFromLocation(locationUserId, user);

            if (locationUser is null)
                return BadRequest();

            return Ok(Json(locationUser).Value);
        }
    }
}
