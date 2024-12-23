using HomeStorage.DataAccess.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeStorage.Logic.Authentication
{
    public class HomeStorageUserManager(
        IUserStore<HomeStorageUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<HomeStorageUser> passwordHasher,
        IEnumerable<IUserValidator<HomeStorageUser>> userValidators,
        IEnumerable<IPasswordValidator<HomeStorageUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<HomeStorageUser>> logger) : UserManager<HomeStorageUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }
}
