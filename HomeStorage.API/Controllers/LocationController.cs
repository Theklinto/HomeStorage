using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.LocationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [Authorize]
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
            LocationModel result = await _locationLogic.CreateLocationAsync(model);
            return Created(Request.GetEncodedUrl(), Json(result).Value);
        }

        [HttpGet("GetLocation/{locationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocation([FromRoute] Guid locationId)
        {
            LocationModel? model = await _locationLogic.GetLocationAsync(locationId);
            if (model is null)
                return NoContent();

            return Ok(Json(model).Value);
        }

        [HttpGet("GetAllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLocations()
        {
            IEnumerable<LocationModel> locations = await _locationLogic.GetAllLocationsByUser();
            return Ok(Json(locations).Value);
        }

        [HttpGet("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetListData()
        {
            IEnumerable<LocationListModel> locations = await _locationLogic.GetList();
            return Ok(Json(locations).Value);
        }
        [HttpPut("UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateLocation([FromForm] LocationUpdateModel model)
        {
            LocationModel? updatedLocation = await _locationLogic.UpdateLocation(model);

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
            (LocationModel? model, bool? allowDeletion) = await _locationLogic.DeleteLocation(locationId);

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
            List<LocationUserListModel> locationUsers = await _locationLogic.GetLocationUsersAsync(locationId);

            return Ok(Json(locationUsers).Value);
        }

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationUserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLocationUser([FromQuery] Guid locationId, [FromQuery] string email)
        {
            LocationUserModel? locationUser = await _locationLogic.AddUserToLocation(locationId, email);

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
            LocationUserModel? locationUser = await _locationLogic.DeleteUserFromLocation(locationUserId);

            if (locationUser is null)
                return BadRequest();

            return Ok(Json(locationUser).Value);
        }
    }
}
