using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeStorage.Logic.Authentication
{
    public class HomeStorageRoleStore(
        HomeStorageDbContext context,
        IdentityErrorDescriber? describer = null) : RoleStore<HomeStorageRole, HomeStorageDbContext, Guid, HomeStorageUserRole, HomeStorageRoleClaim>(context, describer)
    {
    }
}
