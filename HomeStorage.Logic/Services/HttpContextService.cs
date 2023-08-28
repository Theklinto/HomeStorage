using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Services
{
    public class HttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public HttpContextService(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUserAsync()
        {
            string userId = _contextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            IdentityUser? user = await _userManager.FindByIdAsync(userId) ??
                throw new Exception($"Expected user to be logged in. But found no user with (UserId: {userId})" +
                    Environment.NewLine);
            return user;
        }
    }
}
