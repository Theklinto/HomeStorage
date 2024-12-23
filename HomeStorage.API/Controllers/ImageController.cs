using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController(ImageLogic imageLogic) : Controller
    {
        private readonly ImageLogic _imageLogic = imageLogic;

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
