using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models;
using HomeStorage.Logic.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController(ProductLogic productLogic) : Controller
    {
        [HttpGet]
        [Route("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProducts([FromQuery] Guid? locationId, [FromQuery] Guid? categoryId)
        {
            if (categoryId is null && locationId is null)
                return BadRequest();

            List<ProductModel>? products = null;

            if (locationId is not null)
                products = await productLogic.GetProductsFromLocationAsync(locationId.GetValueOrDefault());
            else if (categoryId is not null)
                products = await productLogic.GetProductFromCategoryAsync(categoryId.GetValueOrDefault());

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
            ProductModel? product = await productLogic.GetProductAsync(productId);

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
                .Deserialize<ProductUpdateModel?>(model.Json, Serializer.DefaultJsonSerializerOptions);
            if (updateModel is null)
                return BadRequest();

            updateModel.NewImage = model.File;

            ProductModel? product = await productLogic.CreateProductAsync(updateModel);

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
                .Deserialize<ProductUpdateModel?>(model.Json, Serializer.DefaultJsonSerializerOptions);
            if (updateModel is null)
                return BadRequest();

            updateModel.NewImage = model.File;

            ProductModel? product = await productLogic.UpdateProductAsync(updateModel);

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
            ProductModel? product = await productLogic.DeleteProductAsync(productId);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }
    }
}
