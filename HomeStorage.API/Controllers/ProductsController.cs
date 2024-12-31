using HomeStorage.API.ModelBinders;
using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController(ProductLogic productLogic) : Controller
    {
        private readonly ProductLogic _productLogic = productLogic;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductModel>))]
        public async Task<IActionResult> GetProducts([FromQuery] Guid locationId, [FromQuery] ProductFilterModel filters, CancellationToken cancellationToken = default)
        {
            List<ProductListModel> products = await _productLogic.GetProductsFromLocation(locationId, filters, cancellationToken);

            return Ok(Json(products).Value);
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            ProductModel product = await _productLogic.GetProduct(productId, cancellationToken);

            if (product is null)
                return BadRequest();

            return Ok(Json(product).Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductModel))]
        public async Task<IActionResult> CreateProduct([FromJson] ProductCreateModel model, IFormFile? image, CancellationToken cancellationToken = default)
        {
            model.Image = image;
            ProductModel product = await _productLogic.CreateProduct(model, cancellationToken);

            return Ok(Json(product).Value);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        public async Task<IActionResult> UpdateProduct([FromJson] ProductUpdateModel model, IFormFile image, CancellationToken cancellationToken = default)
        {
            model.Image = image;
            ProductModel product = await _productLogic.UpdateProductAsync(model, cancellationToken);

            return Ok(Json(product).Value);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct([FromQuery] Guid productId, CancellationToken cancellationToken)
        {
            await _productLogic.DeleteProduct(productId, cancellationToken);

            return NoContent();

        }
    }
}
