using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.Logic.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController(ImageLogic imageLogic) : Controller
    {
        private readonly ImageLogic _imageLogic = imageLogic;

        [HttpGet("{imageId:guid}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid imageId)
        {
            Image? image = await _imageLogic.GetImageAsync(imageId);
            if (image is null)
                return NoContent();

            return File(image.ImageBytes, "image/png");
        }
    }
}
