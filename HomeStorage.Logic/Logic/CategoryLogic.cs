using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Models;
using HomeStorage.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class CategoryLogic(HttpContextService contextService, HomeStorageDbContext dbContext) : LogicBase(contextService, dbContext)
    {
        public async Task<List<LookupModel<Guid>>> GetCategoryLookup(Guid locationId, CancellationToken cancellationToken = default)
        {
            await ThrowIfNoAccess<Location>(locationId, Enums.EAccess.Read);

            List<LookupModel<Guid>> categoryLookups = await _db.Categories
                .Where(x => x.LocationId == locationId)
                .Select(x => new LookupModel<Guid>(x.Name, x.CategoryId))
                .ToListAsync(cancellationToken);

            return categoryLookups;
        }

        private async Task<List<Category>> CreateMissingCategories(Guid locationId, List<LookupModel<Guid?>> categoryLookups, CancellationToken cancellationToken = default)
        {
            List<string> categoriesToCreate = categoryLookups
                .Where(x => x.Id == null)
                .Select(x => x.Name)
                .ToList();
            if (categoriesToCreate.Count == 0)
                return [];

            List<Category> categories = categoriesToCreate
                .Select(x => new Category()
                {
                    LocationId = locationId,
                    Name = x,
                })
                .ToList();

            await _db.AddRangeAsync(categories, cancellationToken);

            return categories;
        }

        public async Task<List<Category>> GetCategoriesByLookup(Guid locationId, List<LookupModel<Guid?>> categoryLookups, CancellationToken cancellationToken = default)
        {
            List<Guid> categoryIds = categoryLookups
                .Where(x => x.Id != null)
                .Select(x => x.Id)
                .Cast<Guid>()
                .ToList();

            List<Category> categories = [];

            if (categoryIds.Count > 0)
                categories = await _db.Categories
                   .Where(x => EF.Constant(categoryIds).Contains(x.CategoryId) && locationId == x.LocationId)
                   .ToListAsync(cancellationToken);

            List<Category> missingCategories = await CreateMissingCategories(locationId, categoryLookups, cancellationToken);

            categories.AddRange(missingCategories);

            return categories;
        }
    }
}
