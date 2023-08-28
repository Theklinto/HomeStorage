using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Abstracts
{
    public abstract class LogicBase
    {
        protected readonly HttpContextService _httpContextService;
        protected readonly HomeStorageDbContext _db;

        protected LogicBase(HttpContextService httpContextService, HomeStorageDbContext db)
        {
            _httpContextService = httpContextService;
            _db = db;
        }

        protected async Task<IdentityUser> GetCurrentUser() => await _httpContextService.GetCurrentUserAsync();
        protected async Task<bool> CheckUserAccess<T>(Guid? id, EAccess access, IdentityUser? user = default)
        {
            if (id.HasValue is false)
                return false;

            IQueryable<LocationUser> query = typeof(T).Name switch
            {
                nameof(Product) => _db.Products
                    .Where(x => x.ProductId == id.Value)
                    .SelectMany(x => x.Location.LocationUsers),
                nameof(Category) => _db.Categories
                    .Where(x => x.CategoryId == id.Value)
                    .Take(1)
                    .Select(x => x.Location)
                    .SelectMany(x => x.LocationUsers),
                nameof(Location) => _db.Locations
                    .Where(x => x.LocationId == id.Value)
                    .Take(1)
                    .SelectMany(x => x.LocationUsers),
                _ => throw new NotImplementedException()
            };

            user ??= await GetCurrentUser();
            Expression<Func<LocationUser, bool>> selector = access switch
            {
                EAccess.LocationAdmin or
                EAccess.Update or
                EAccess.Delete or
                EAccess.Create => (x) => x.UserId == user.Id && (x.IsLocationOwner || x.IsLoactionAdmin),
                _ => (x) => x.UserId == user.Id,
            };

            bool hasAccess = await query.AnyAsync(selector);
            return hasAccess;
        }
    }
}
