using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models;
using HomeStorage.Logic.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductLogic _productLogic;


        public ProductController(ProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProducts([FromQuery] Guid? locationId, [FromQuery] Guid? categoryId,
            [FromQuery] string searchExpression = "")
        {
            if (categoryId is null && locationId is null)
                return BadRequest();

            List<ProductModel>? products = null;

            if (locationId is not null)
                products = await _productLogic.GetProductsFromLocationAsync(locationId.GetValueOrDefault(), searchExpression);
            else if (categoryId is not null)
                products = await _productLogic.GetProductFromCategoryAsync(categoryId.GetValueOrDefault(), searchExpression);

            if (products is null)
                return BadRequest();

            return Ok(Json(products).Value);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProduct([FromQuery] Guid productId)
        {
            ProductModel? product = await _productLogic.GetProductAsync(productId);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProduct([FromForm] JsonWithFileModel model)
        {
            ProductUpdateModel? updateModel = JsonSerializer
                .Deserialize<ProductUpdateModel?>(model.Json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (updateModel is null)
                return BadRequest();

            updateModel.NewImage = model.File;

            ProductModel? product = await _productLogic.CreateProductAsync(updateModel);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProduct([FromForm] JsonWithFileModel model)
        {
            ProductUpdateModel? updateModel = JsonSerializer
                .Deserialize<ProductUpdateModel?>(model.Json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (updateModel is null)
                return BadRequest();

            updateModel.NewImage = model.File;

            ProductModel? product = await _productLogic.UpdateProductAsync(updateModel);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProduct([FromQuery] Guid productId)
        {
            ProductModel? product = await _productLogic.DeleteProductAsync(productId);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }
    }
}
