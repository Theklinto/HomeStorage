using AutoMapper;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Logic
{
    public class ProductLogic
    {
        private readonly HomeStorageDbContext _db;
        private readonly ImageLogic _imageLogic;
        private readonly LocationLogic _locationLogic;
        private readonly IMapper _mapper;
        public ProductLogic(HomeStorageDbContext db, IMapper mapper, LocationLogic locationLogic, ImageLogic imageLogic)
        {
            _db = db;
            _mapper = mapper;
            _locationLogic = locationLogic;
            _imageLogic = imageLogic;
        }

        public async Task<ProductModel?> GetProductAsync(Guid productId, IdentityUser user)
        {
            Product? product = await _db.Products
                .Include(x => x.Location)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            bool hasAccess = await _locationLogic.CheckUserAccess(product?.Location, user, Enums.EAccess.Read);

            if (product is null)
                return null;

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<List<ProductModel>?> GetProductsFromLocationAsync(Guid locationId, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            bool hasAccess = await _locationLogic.CheckUserAccess(location, user, Enums.EAccess.Read);

            if (location is null || hasAccess is false)
                return null;

            return _mapper.Map<List<ProductModel>>(location?.Products);
        }

        public async Task<List<ProductModel>?> GetProductFromCategoryAsync(Guid categoryId, IdentityUser user)
        {
            Category? category = await _db.Categories
                .Include(x => x.Products)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            bool hasAccess = await _locationLogic.CheckUserAccess(category?.Location, user, Enums.EAccess.Read);

            if (category is null || hasAccess is false)
                return null;

            return _mapper.Map<List<ProductModel>>(category?.Products);
        }

        public async Task<ProductModel?> CreateProductAsync(ProductUpdateModel model, IdentityUser user)
        {
            //Find by locationId
            Location? location = await _db.Locations
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);
            //Find by categoryId
            if (location is null)
            {
                Guid? categoryId = model.Categories.FirstOrDefault()?.CategoryId;
                location = await _db.Categories
                    .Where(x => x.CategoryId == categoryId)
                    .Select(x => x.Location)
                    .Include(x => x.Products)
                    .FirstOrDefaultAsync();
            }

            bool hasAccess = await _locationLogic.CheckUserAccess(location, user, Enums.EAccess.Create);

            if (location is null || hasAccess is false)
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
                    await _imageLogic.CreateImageAsync(model.NewImage, user) :
                    null,
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<ProductModel?> UpdateProductAsync(ProductUpdateModel model, IdentityUser user)
        {
            Product? product = await _db.Products
                .Include(x => x.Location)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

            bool hasAccess = await _locationLogic.CheckUserAccess(product?.Location, user, Enums.EAccess.Create);

            if (product is null || hasAccess is false)
                return null;

            List<Guid> categoryIds = model.Categories.Select(x => x.CategoryId).ToList();
            List<Category> categories = await _db.Categories
                .Where(x => categoryIds.Contains(x.CategoryId))
                .ToListAsync();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Categories = categories;
            product.Amount = model.Amount;
            product.ExpirationDate = model.ExpirationDate;

            if (product.Image is null && model.NewImage is not null)
                product.ImageId = await _imageLogic.CreateImageAsync(model.NewImage, user);
            else if (model.NewImage is not null)
                product.Image = await _imageLogic.UpdateImageAsync(product.ImageId.GetValueOrDefault(), model.NewImage);

            await _db.SaveChangesAsync();

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<ProductModel?> DeleteProductAsync(Guid productId, IdentityUser user)
        {
            Product? product = await _db.Products
                .Include(x => x.Location)
                .Include(x => x.Categories)
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            bool hasAccess = await _locationLogic.CheckUserAccess(product?.Location, user, Enums.EAccess.Delete);

            if (product is null || hasAccess is false)
                return null;

            _db.Products.Remove(product);

            await _db.SaveChangesAsync();

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<bool> UpdateProductAmount(Guid productId, double newAmount, IdentityUser user)
        {
            Product? product = await _db.Products
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            bool hasAccess = await _locationLogic.CheckUserAccess(product?.Location, user, Enums.EAccess.Delete);

            if (product is null || hasAccess is false)
                return false;

            product.Amount = newAmount;
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
