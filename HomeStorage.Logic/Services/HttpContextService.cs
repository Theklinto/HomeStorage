using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HomeStorage.Logic.Services
{
    public class HttpContextService(IHttpContextAccessor contextAccessor, UserManager<HomeStorageUser> userManager)
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly UserManager<HomeStorageUser> _userManager = userManager;

        public async Task<HomeStorageUser> GetCurrentUserAsync()
        {
            string userId = _contextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            if (Guid.TryParse(userId, out Guid id) is false)
                throw new NotAuthenticatedException("");

            HomeStorageUser user = await _userManager.FindByIdAsync(id.ToString()) ??
                throw new NotAuthenticatedException($"Expected user to be logged in. But found no user with (UserId: {userId})");
            return user;
        }
    }
}
