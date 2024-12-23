using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeStorage.Logic.Authentication
{
    public class HomeStorageUserStore(
        HomeStorageDbContext context,
        IdentityErrorDescriber? describer = null) : UserStore<HomeStorageUser, HomeStorageRole, HomeStorageDbContext, Guid, HomeStorageUserClaim, HomeStorageUserRole, HomeStorageUserLogin, HomeStorageUserToken, HomeStorageRoleClaim>(context, describer)
    {
    }
}
