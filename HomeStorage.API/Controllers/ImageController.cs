using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly ImageLogic _imageLogic;

        public ImageController(ImageLogic imageLogic)
        {
            _imageLogic = imageLogic;
        }

        [Route("{imageId}")]
        [HttpGet]
        public async Task<IActionResult> GetImage(Guid imageId)
        {
            Image? image = await _imageLogic.GetImageAsync(imageId);
            if (image is null)
                return NoContent();

            return File(image.ImageBytes, "image/png");
        }
    }
}
