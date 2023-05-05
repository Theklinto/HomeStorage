using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ImageLogic _imageLogic;
        public ImageController(UserManager<IdentityUser> userManager, ImageLogic imageLogic)
        {
            _userManager = userManager;
            _imageLogic = imageLogic;
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> CreateImage(IFormFile file)
        //{
        //    IdentityUser? user = await this.GetCurrentUser(_userManager);
        //    if (user is null)
        //        return Unauthorized();

        //    Guid? imageId = await _imageLogic.CreateImageAsync(file., user);
        //    if(imageId is null || imageId == Guid.Empty)
        //        return BadRequest();

        //    return Created(Request.GetEncodedUrl(), imageId);

        //}

        [AllowAnonymous]
        [HttpGet("{imageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PhysicalFileResult))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetImage(Guid imageId)
        {
            string imagePath = await _imageLogic.GetImagePathAsync(imageId);

            if (string.IsNullOrWhiteSpace(imagePath))
                return NoContent();

            try
            {
                PhysicalFileResult result = PhysicalFile(imagePath, "image/png");
                return result;
            }
            catch
            {
                return NoContent();
            }
        }
    }
}
