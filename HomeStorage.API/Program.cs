using HomeStorage.API.Authentication;
using HomeStorage.API.ExceptionHandlers;
using HomeStorage.API.Interfaces;
using HomeStorage.API.Middleware;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Logic;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
#if RELEASE
    .AddJsonFile("appsettings.Production.json")
#else
    .AddJsonFile($"appsettings.Development.json")
#endif
    .Build();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "HomeStorageAPI", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new()
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<HomeStorageDbContext>(options => options.UseSqlServer(config.GetConnectionString("Default")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HttpContextService>();

#region Logics

builder.Services.AddTransient<AuthenticationLogic>();
builder.Services.AddTransient<LocationLogic>();
builder.Services.AddTransient<ImageLogic>();
builder.Services.AddTransient<CategoryLogic>();
builder.Services.AddTransient<ProductLogic>();


#region Exception Handlers
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddTransient<IMiddlewareExceptionHandler, NotAuthenticatedExceptionHandler>();
builder.Services.AddTransient<IMiddlewareExceptionHandler, NotAuthorizedExceptionHandler>();
#endregion

#endregion

#region Authentication

builder.Services.AddHomeStorageIdentity();
builder.Services.AddHomeStorageAuth(config);


//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin =>
        {
#if !DEBUG
            List<string> allowedOrigins = new(config
                .GetValue<string>("AllowedOriginDomains")?
                .Split(";") ?? Enumerable.Empty<string>());

            if (allowedOrigins.Contains(new Uri(origin).Host))
                return true;
            return false;
#else
            return true;
#endif
        });
    });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

#endregion

var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<HomeStorageDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#if !DEBUG
app.UseHttpsRedirection();
app.UseCookiePolicy(new()
{
    Secure = CookieSecurePolicy.Always
});
#else
#endif

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseExceptionMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
