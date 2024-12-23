using HomeStorage.DataAccess.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HomeStorage.Logic.Authentication
{
    public class HomeStorageRoleManager(
        IRoleStore<HomeStorageRole> store,
        IEnumerable<IRoleValidator<HomeStorageRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<HomeStorageRole>> logger) : RoleManager<HomeStorageRole>(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}
