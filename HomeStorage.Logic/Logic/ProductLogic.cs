using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.ProductEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Exceptions;
using HomeStorage.Logic.IQueryableExtensions;
using HomeStorage.Logic.Models;
using HomeStorage.Logic.Models.ProductModels;
using HomeStorage.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class ProductLogic(HomeStorageDbContext db, ImageLogic imageLogic, HttpContextService contextService, CategoryLogic categoryLogic) : LogicBase(contextService, db)
    {
        private readonly ImageLogic _imageLogic = imageLogic;
        private readonly CategoryLogic _categoryLogic = categoryLogic;

        public async Task<ProductModel> GetProduct(Guid productId, CancellationToken cancellationToken)
        {
            await ThrowIfNoAccess<Product>(productId, EAccess.Read);

            ProductModel product = await _db.Products
                .Select(x => new ProductModel()
                {
                    Name = x.Name,
                    Categories = x.Categories.Select(x => new LookupModel<Guid>(x.Name, x.CategoryId)).ToList(),
                    Amount = x.Amount,
                    Description = x.Description,
                    ExpirationDate = x.ExpirationDate,
                    ImageUrl = x.Image != null ? ImageLogic.GetImageUrl(x.Image.ImageId, x.Image.LastModified) : null,
                    LocationId = x.LocationId,
                    ProductId = x.ProductId
                })
                .FirstAsync(x => x.ProductId == productId, cancellationToken);

            return product;
        }

        public async Task<List<ProductListModel>> GetProductsFromLocation(Guid locationId, ProductFilterModel filters, CancellationToken cancellationToken = default)
        {
            await ThrowIfNoAccess<Location>(locationId, EAccess.Read);

            IQueryable<Product> query = _db.Products
                .Where(x => x.LocationId == locationId);

            if (filters.Categories.Count > 0)
                query = query
                    .Where(x => x.Categories.Any(y => EF.Constant(filters.Categories).Contains(y.CategoryId)));

            if (filters.MinAmount is not null)
                query = query
                    .Where(x => x.Amount >= filters.MinAmount);
            if (filters.MaxAmount is not null)
                query = query
                    .Where(x => x.Amount <= filters.MaxAmount || x.Amount == null);

            if (string.IsNullOrWhiteSpace(filters.SearchString) is false)
            {
                string searchExpr = $"%%{filters.SearchString}%%";
                query = query
                    .Where(x => EF.Functions.Like(x.Name, searchExpr));
            }

            List<ProductListModel> products = await query
                .OrderByProperty(filters.OrderByProperty, filters.SortDirection, x => x.ProductId)
                .Select(x => new ProductListModel()
                {
                    Name = x.Name,
                    ProductId = x.ProductId,
                    Amount = x.Amount,
                    Description = x.Description,
                    ExpirationDate = x.ExpirationDate,
                    ImageUrl = x.Image != null ? ImageLogic.GetImageUrl(x.Image.ImageId, x.Image.LastModified) : null
                })
                .ToListAsync(cancellationToken);

            return products;
        }

        public async Task<ProductModel> CreateProduct(ProductCreateModel model, CancellationToken cancellationToken)
        {
            await ThrowIfNoAccess<Location>(model.LocationId, EAccess.Read);

            List<Category> categories = await _categoryLogic.GetCategoriesByLookup(model.LocationId, model.Categories, cancellationToken);

            Product product = new()
            {
                ProductId = Guid.NewGuid(),
                Amount = model.Amount,
                Description = model.Description,
                Name = model.Name,
                ExpirationDate = model.ExpirationDate,
                LocationId = model.LocationId,
                Categories = categories
            };

            if (model.Image is not null)
                product.Image = await _imageLogic.CreateOrUpdateImage(model.Image, null, cancellationToken);

            await _db.Products.AddAsync(product, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            ProductModel createdProduct = new()
            {
                Name = product.Name,
                LocationId = product.LocationId,
                ProductId = product.ProductId,
                Categories = product.Categories.Select(x => new LookupModel<Guid>(x.Name, x.CategoryId)).ToList(),
                Description = product.Description,
                ExpirationDate = product.ExpirationDate,
                ImageUrl = ImageLogic.GetImageUrl(product.Image?.ImageId, product.Image?.LastModified),
                Amount = product.Amount,

            };

            return createdProduct;
        }

        public async Task<ProductModel> UpdateProductAsync(ProductUpdateModel model, CancellationToken cancellationToken)
        {
            await ThrowIfNoAccess<Product>(model.ProductId, EAccess.Update);

            Product product = await _db.Products
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == model.ProductId, cancellationToken: cancellationToken) ?? throw new NotFoundException();


            List<Category> categories = await _categoryLogic.GetCategoriesByLookup(model.LocationId, model.Categories, cancellationToken);

            product.Categories = categories;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Amount = model.Amount;
            product.ExpirationDate = model.ExpirationDate;

            if (model.Image is not null)
                product.Image = await _imageLogic.CreateOrUpdateImage(model.Image, product.ImageId, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            ProductModel updatedProduct = new()
            {
                LocationId = product.LocationId,
                Name = product.Name,
                ProductId = product.ProductId,
                Categories = product.Categories.Select(x => new LookupModel<Guid>(x.Name, x.CategoryId)).ToList(),
                Amount = product.Amount,
                ExpirationDate = product.ExpirationDate,
                Description = product.Description,
                ImageUrl = ImageLogic.GetImageUrl(product.Image?.ImageId, product.Image?.LastModified)
            };

            return updatedProduct;
        }

        public async Task DeleteProduct(Guid productId, CancellationToken cancellationToken)
        {
            await ThrowIfNoAccess<Product>(productId, EAccess.Delete);

            Product product = await _db.Products
                .Include(x => x.Categories)
                .FirstAsync(x => x.ProductId == productId, cancellationToken);

            _db.Products.Remove(product);
            await _db.SaveChangesAsync(cancellationToken);
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
