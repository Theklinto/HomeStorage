using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Models.ProductModels;
using HomeStorage.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class ProductLogic : LogicBase
    {
        private readonly ImageLogic _imageLogic;
        private readonly LocationLogic _locationLogic;
        public ProductLogic(HomeStorageDbContext db, LocationLogic locationLogic, ImageLogic imageLogic, HttpContextService contextService)
            : base(contextService, db)
        {
            _locationLogic = locationLogic;
            _imageLogic = imageLogic;
        }

        public async Task<ProductModel?> GetProductAsync(Guid productId)
        {
            if (await CheckUserAccess<Product>(productId, Enums.EAccess.Read) is false)
                return null;

            Product? product = await _db.Products
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product is null)
                return null;

            return DTOService.AsDTO<ProductModel, Product>(product);
        }

        public async Task<List<ProductModel>?> GetProductsFromLocationAsync(Guid locationId, string searchExpression)
        {
            if (await CheckUserAccess<Location>(locationId, EAccess.Read) is false)
                return null;

            List<Product> products = await _db.Locations
                .Where(x => x.LocationId == locationId)
                .SelectMany(x => x.Products)
                .ToListAsync();

            return products
                .Select(DTOService.AsDTO<ProductModel, Product>)
                .ToList();
        }

        public async Task<List<ProductModel>?> GetProductFromCategoryAsync(Guid categoryId, string searchExpression)
        {
            if (await CheckUserAccess<Category>(categoryId, EAccess.Read) is false)
                return null;

            List<Product> products = await _db.Categories
                .Where(x => x.CategoryId == categoryId)
                .SelectMany(x => x.Products)
                .Where(Product.ContainsSearchString(searchExpression))
                .ToListAsync();

            return products
                .Select(DTOService.AsDTO<ProductModel, Product>)
                .ToList();
        }

        public async Task<ProductModel?> CreateProductAsync(ProductUpdateModel model)
        {

            if (await CheckUserAccess<Location>(model.LocationId, EAccess.Create) is false)
                return null;

            Location? location = await _db.Locations
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);

            if (location is null)
                return null;

            List<Guid> categoryIds = model.Categories.Select(x => x.CategoryId).ToList();
            List<Category> categories = await _db.Categories
                .Where(x => categoryIds.Contains(x.CategoryId))
                .ToListAsync();

            Product product = new()
            {
                ProductId = Guid.NewGuid(),
                Amount = model.Amount,
                Description = model.Description,
                Name = model.Name,
                ExpirationDate = model.ExpirationDate,
                LocationId = location.LocationId,
                Categories = categories,
                ImageId = model.NewImage is not null ?
                    await _imageLogic.CreateImageAsync(model.NewImage) :
                    null,
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return DTOService.AsDTO<ProductModel, Product>(product);
        }

        public async Task<ProductModel?> UpdateProductAsync(ProductUpdateModel model)
        {
            if (await CheckUserAccess<Product>(model.ProductId, EAccess.Update) is false)
                return null;

            Product? product = await _db.Products
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

            if (product is null)
                return null;

            List<Guid> categoryIds = model.Categories.Select(x => x.CategoryId).ToList();
            List<Category> categories = await _db.Categories
                .Where(x => categoryIds.Contains(x.CategoryId))
                .ToListAsync();

            //Remove old categories

            product.Categories.Clear();
            product.Categories.AddRange(categories);

            product.Name = model.Name;
            product.Description = model.Description;
            product.Amount = model.Amount;
            product.ExpirationDate = model.ExpirationDate;

            if (product.Image is null && model.NewImage is not null)
                product.ImageId = await _imageLogic.CreateImageAsync(model.NewImage);
            else if (model.NewImage is not null)
                product.Image = await _imageLogic.UpdateImageAsync(product.ImageId.GetValueOrDefault(), model.NewImage);

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<ProductModel, Product>(product);
        }

        public async Task<ProductModel?> DeleteProductAsync(Guid productId)
        {
            if (await CheckUserAccess<Product>(productId, EAccess.Delete) is false)
                return null;

            Product? product = await _db.Products
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product is null)
                return null;

            product.Categories.Clear();
            _db.Products.Remove(product);

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<ProductModel, Product>(product);
        }

        public async Task<bool> UpdateProductAmount(Guid productId, double newAmount)
        {
            if (await CheckUserAccess<Product>(productId, EAccess.Update) is false)
                return false;

            Product? product = await _db.Products
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product is null)
                return false;

            product.Amount = newAmount;
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
