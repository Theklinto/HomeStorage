using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeStorage.API
{
    public static class Extensions
    {
        public static async Task<IdentityUser> GetCurrentUser(this Controller controller, UserManager<IdentityUser> userManager)
        {
            string userId = controller.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            IdentityUser? user = await userManager.FindByIdAsync(userId) ?? 
                throw new Exception($"Expected user to be logged in. But found no user with (UserId: {userId})" +
                    Environment.NewLine);
            return user;
        }
    }
}
