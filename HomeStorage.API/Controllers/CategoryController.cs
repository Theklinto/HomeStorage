using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.CategoryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic _categoryLogic;
        public CategoryController(CategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        [Route("List/{locationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesForLocation([FromRoute] Guid locationId)
        {
            List<CategoryModel>? categories = await _categoryLogic.GetCategoriesForLocation(locationId);

            if (categories is null)
                return BadRequest();

            return Ok(Json(categories).Value);
        }

        [HttpGet]
        [Route("{categoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid categoryId)
        {
            CategoryModel? category = await _categoryLogic.GetCategory(categoryId);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryUpdateModel model)
        {
            CategoryModel? category = await _categoryLogic.CreateCategory(model);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateModel model)
        {
            CategoryModel? category = await _categoryLogic.UpdateCategory(model);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }

        [HttpDelete]
        [Route("Delete/{categoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            CategoryModel? category = await _categoryLogic.DeleteCategory(categoryId);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }

        [HttpGet]
        [Route("lookup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryNotationModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesLookup([FromQuery]Guid locationId)
        {
            List<CategoryNotationModel>? category = await _categoryLogic
                .GetCategoriesAsNotationForLocationAsync(locationId);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }
    }
}
