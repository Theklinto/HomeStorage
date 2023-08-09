using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.Category;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CategoryLogic _categoryLogic;
        public CategoryController(UserManager<IdentityUser> userManager, CategoryLogic categoryLogic)
        {
            _userManager = userManager;
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        [Route("List/{locationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesForLocation([FromRoute] Guid locationId)
        {
            IdentityUser user = await this.GetCurrentUser(_userManager);

            List<CategoryModel>? categories = await _categoryLogic.GetCategoriesForLocation(locationId, user);

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
            IdentityUser user = await this.GetCurrentUser(_userManager);

            CategoryModel? category = await _categoryLogic.GetCategory(categoryId, user);

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
            IdentityUser user = await this.GetCurrentUser(_userManager);

            CategoryModel? category = await _categoryLogic.CreateCategory(model, user);

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
            IdentityUser user = await this.GetCurrentUser(_userManager);

            CategoryModel? category = await _categoryLogic.UpdateCategory(model, user);

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
            IdentityUser user = await this.GetCurrentUser(_userManager);

            CategoryModel? category = await _categoryLogic.DeleteCategory(categoryId, user);

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
            IdentityUser user = await this.GetCurrentUser(_userManager);

            List<CategoryNotationModel>? category = await _categoryLogic
                .GetCategoryAsNocationForLocationAsync(locationId, user);

            if (category is null)
                return BadRequest();

            return Ok(Json(category).Value);
        }
    }
}
