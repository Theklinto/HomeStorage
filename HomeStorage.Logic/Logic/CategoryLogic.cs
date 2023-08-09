using AutoMapper;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Models.Category;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Logic
{
    public class CategoryLogic
    {
        private readonly HomeStorageDbContext _db;
        private readonly IMapper _mapper;
        private readonly ImageLogic _imageLogic;
        private readonly LocationLogic _locationLogic;
        public CategoryLogic(HomeStorageDbContext db, IMapper mapper, ImageLogic imageLogic, LocationLogic locationLogic)
        {
            _db = db;
            _mapper = mapper;
            _imageLogic = imageLogic;
            _locationLogic = locationLogic;
        }

        public async Task<CategoryModel?> GetCategory(Guid categoryId, IdentityUser user)
        {
            Category? category = await _db.Categories
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            bool hasAccess = await _locationLogic.CheckUserAccess(category?.Location, user, EAccess.Read);

            if (category is null || hasAccess is false)
                return null;

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<List<CategoryModel>?> GetCategoriesForLocation(Guid locationId, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            bool hasAccess = await _locationLogic.CheckUserAccess(location, user, EAccess.Read);

            if (location is null || hasAccess is false)
                return null;

            return _mapper.Map<List<CategoryModel>>(location.Categories);
        }

        public async Task<CategoryModel?> CreateCategory(CategoryUpdateModel model, IdentityUser user)
        {
            Location? location = await _db.Locations
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);

            bool hasAccess = await _locationLogic.CheckUserAccess(location, user, EAccess.Create);

            if (location is null || hasAccess is false)
                return null;

            Category category = new()
            {
                Name = model.Name,
                LocationId = model.LocationId,
            };

            if (model.NewImage is not null)
                category.ImageId = await _imageLogic.CreateImageAsync(model.NewImage, user);

            location.Categories.Add(category);

            await _db.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel?> UpdateCategory(CategoryUpdateModel model, IdentityUser user)
        {
            Category? category = await _db.Categories
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId);

            bool hasAccess = await _locationLogic.CheckUserAccess(category?.Location, user, EAccess.Update);

            if (category is null || hasAccess is false)
                return null;

            if (model.NewImage is not null && category.Image is not null)
                category.Image = await _imageLogic.UpdateImageAsync(category.ImageId.GetValueOrDefault(), model.NewImage);
            else if (model.NewImage is not null)
                category.ImageId = await _imageLogic.CreateImageAsync(model.NewImage, user);

            category.Name = model.Name;

            await _db.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel?> DeleteCategory(Guid categoryId, IdentityUser user)
        {
            Category? category = await _db.Categories
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            bool hasAccess = await _locationLogic.CheckUserAccess(category?.Location, user, EAccess.Delete);

            if (category is null || hasAccess is false)
                return null;

            if (category.Image is not null)
                _db.Images.Remove(category.Image);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<List<CategoryNotationModel>?> GetCategoryAsNocationForLocationAsync(Guid locationId, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            bool hasAccess = await _locationLogic.CheckUserAccess(location, user, EAccess.Read);

            if(location is null || hasAccess is false)
                return null;

            return _mapper.Map<List<CategoryNotationModel>>(location.Categories);
        }
    }
}
