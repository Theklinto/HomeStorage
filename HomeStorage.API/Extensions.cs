using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeStorage.API
{
    public static class Extensions
    {
        public static async Task<IdentityUser?> GetCurrentUser(this Controller controller, UserManager<IdentityUser> userManager) => 
            await userManager.FindByNameAsync(controller.User?.Identity?.Name ?? string.Empty);
    }
}
