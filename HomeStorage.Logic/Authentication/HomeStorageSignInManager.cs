using HomeStorage.DataAccess.UserEntities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeStorage.Logic.Authentication
{
    public class HomeStorageSignInManager(
        UserManager<HomeStorageUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<HomeStorageUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<HomeStorageUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<HomeStorageUser> confirmation) : SignInManager<HomeStorageUser>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}
