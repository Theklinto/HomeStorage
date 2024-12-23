using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Authentication;
using HomeStorage.Logic.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace HomeStorage.API.Authentication;

public static class ServiceBuilder
{
    public static void AddHomeStorageIdentity(this IServiceCollection services)
    {
        services.AddScoped<HomeStorageUserStore>();
        services.AddScoped<HomeStorageUserManager>();
        services.AddScoped<HomeStorageRoleManager>();
        services.AddScoped<HomeStorageRoleStore>();
        services.AddScoped<HomeStorageSignInManager>();

        services.AddIdentity<HomeStorageUser, HomeStorageRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
            .AddUserStore<HomeStorageUserStore>()
            .AddUserManager<HomeStorageUserManager>()
            .AddRoleStore<HomeStorageRoleStore>()
            .AddRoleManager<HomeStorageRoleManager>()
            .AddSignInManager<HomeStorageSignInManager>()
            .AddEntityFrameworkStores<HomeStorageDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddHomeStorageAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorization(options =>
        {
            AuthorizationPolicyBuilder policyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(ClaimTypes.NameIdentifier)
                .RequireAuthenticatedUser();
            options.DefaultPolicy = policyBuilder.Build();
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string secret = config["JWTSettings:Secret"]
                    ?? throw new Exception("JWT Secret not set! (JWTSettings:Secret)");
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = config["JWTSettings:Issuer"],
                    //ValidAudience = config["JWTSettings:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };

            });
    }
}
