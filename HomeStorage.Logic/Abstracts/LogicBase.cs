using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.ProductEntities;
using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Exceptions;
using HomeStorage.Logic.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HomeStorage.Logic.Abstracts
{
    public abstract class LogicBase(HttpContextService httpContextService, HomeStorageDbContext db)
    {
        protected readonly HomeStorageDbContext _db = db;
        protected async Task<HomeStorageUser> GetCurrentUser() => await httpContextService.GetCurrentUserAsync();
        protected async Task<bool> CheckUserAccess<T>(Guid? id, EAccess access, HomeStorageUser? user = default)
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="access"></param>
        /// <param name="user"></param>
        /// <exception cref="NotAuthorizedException"></exception>
        protected async Task ThrowIfNoAccess<T>(Guid? id, EAccess access, HomeStorageUser? user = default)
        {
            bool hasAccess = await CheckUserAccess<T>(id, access, user);
            if (hasAccess is false)
                throw new NotAuthorizedException("You don't have access to the required ressource");
        }
    }
}
