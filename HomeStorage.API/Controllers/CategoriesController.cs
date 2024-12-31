using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(CategoryLogic categoryLogic) : Controller
    {
        private readonly CategoryLogic _categoryLogic = categoryLogic;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LookupModel<Guid>>))]
        public async Task<IActionResult> Lookup([FromQuery] Guid locationId)
        {
            List<LookupModel<Guid>> lookups = await _categoryLogic.GetCategoryLookup(locationId);

            return Ok(lookups);
        }
    }
}
