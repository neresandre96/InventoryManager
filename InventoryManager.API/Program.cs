using InventoryManager.API.Data.App.Context;
using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Entities;
using InventoryManager.API.Data.App.Services;
using InventoryManager.API.Data.Auth.Context;
using InventoryManager.API.Data.Auth.Interfaces;
using InventoryManager.API.Data.Auth.Models;
using InventoryManager.API.Handlers;
using InventoryManager.API.Middlewares;
using InventoryManager.API.Providers;
using InventoryManager.API.Providers.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.AddScoped<IRequestHandler<Ingredient>, RequestHandler<Ingredient>>();
builder.Services.AddScoped<IRequestHandler<Movement>, RequestHandler<Movement>>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IMovementService, MovementService>();
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IUserDbContext, UserDbContext>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionStrings["AppDb"]),
    ServiceLifetime.Scoped);

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(connectionStrings["UserDb"]));

builder.Services.AddIdentity<User, Role>()
    .AddRoles<Role>()
    .AddApiEndpoints()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

builder.Services.AddScoped<RoleManager<Role>>();

builder.Services.AddScoped<AuthProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

var authGroup = app.MapGroup("auth");

authGroup.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync("Identity.Application");
    return Results.NoContent();
}).WithTags("Authentication");

authGroup.MapIdentityApi<User>().WithTags("Authentication");

app.Run();
