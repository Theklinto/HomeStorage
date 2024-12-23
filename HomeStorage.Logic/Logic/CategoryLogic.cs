using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Models.CategoryModels;
using HomeStorage.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class CategoryLogic(HomeStorageDbContext db, ImageLogic imageLogic,
        HttpContextService httpContextService) : LogicBase(httpContextService, db)
    {
        private readonly ImageLogic _imageLogic = imageLogic;

        public async Task<CategoryModel?> GetCategory(Guid categoryId)
        {
            Category? category = await _db.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            bool hasAccess = await CheckUserAccess<Category>(categoryId, EAccess.Read);

            if (category is null || hasAccess is false)
                return null;

            return DTOService.AsDTO<CategoryModel, Category>(category);
        }

        public async Task<List<CategoryModel>?> GetCategoriesForLocation(Guid locationId)
        {
            List<Category> categories = await _db.Categories
                .Where(x => x.LocationId == locationId)
                .ToListAsync();

            bool hasAccess = await CheckUserAccess<Location>(locationId, EAccess.Read);

            if (hasAccess is false)
                return null;

            return categories
                .Select(DTOService.AsDTO<CategoryModel, Category>)
                .ToList();
        }

        public async Task<CategoryModel?> CreateCategory(CategoryUpdateModel model)
        {
            Location? location = await _db.Locations
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);

            bool hasAccess = await CheckUserAccess<Location>(model.LocationId, EAccess.Create);

            if (location is null || hasAccess is false)
                return null;

            Category category = new()
            {
                Name = model.Name,
                LocationId = model.LocationId,
            };

            if (model.NewImage is not null)
                category.ImageId = await _imageLogic.CreateImageAsync(model.NewImage);

            location.Categories.Add(category);

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<CategoryModel, Category>(category);
        }

        public async Task<CategoryModel?> UpdateCategory(CategoryUpdateModel model)
        {
            Category? category = await _db.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId);

            bool hasAccess = await CheckUserAccess<Category>(model.CategoryId, EAccess.Update);

            if (category is null || hasAccess is false)
                return null;

            if (model.NewImage is not null && category.Image is not null)
                category.Image = await _imageLogic.UpdateImageAsync(category.ImageId.GetValueOrDefault(), model.NewImage);
            else if (model.NewImage is not null)
                category.ImageId = await _imageLogic.CreateImageAsync(model.NewImage);

            category.Name = model.Name;

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<CategoryModel, Category>(category);
        }

        public async Task<CategoryModel?> DeleteCategory(Guid categoryId)
        {
            if (await CheckUserAccess<Category>(categoryId, EAccess.Delete) is false)
                return null;

            Category? category = await _db.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            if (category is null)
                return null;

            if (category.Image is not null)
                _db.Images.Remove(category.Image);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return DTOService.AsDTO<CategoryModel, Category>(category);
        }

        public async Task<List<CategoryNotationModel>?> GetCategoriesAsNotationForLocationAsync(Guid locationId)
        {
            bool hasAccess = await CheckUserAccess<Location>(locationId, EAccess.Read);

            List<Category> categories = await _db.Categories
                .Where(x => x.LocationId == locationId)
                .ToListAsync();

            if (hasAccess is false)
                return null;

            return categories
                .Select(DTOService.AsDTO<CategoryNotationModel, Category>)
                .ToList();
        }
    }
}
