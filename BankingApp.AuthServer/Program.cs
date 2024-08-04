using BankingApp.AuthServer.AppConfiguration;
using BankingApp.AuthServer.Data;
using BankingApp.AuthServer.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantBooking.AuthServer.Data;
using System.Reflection;
using System.Security.Claims;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString(nameof(IdentityServerAppContext));
builder.Services.AddDbContext<IdentityServerAppContext>(config =>
    {
        //config.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        config.UseInMemoryDatabase("MEMORY");
    })
    .AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequireDigit = false;
        config.Password.RequireLowercase = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequiredLength = 6;
    })
    //.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityServerAppContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddIdentityServer(config =>
    {
        config.UserInteraction.LoginUrl = "/Auth/Login";
        config.UserInteraction.LogoutUrl = "/Auth/Logout";
    })
    .AddAspNetIdentity<IdentityUser>()
    //.AddConfigurationStore(options =>
    //{
    //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
    //        sql => sql.MigrationsAssembly(migrationsAssembly));
    //})
    //.AddOperationalStore(options =>
    //{
    //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
    //        sql => sql.MigrationsAssembly(migrationsAssembly));
    //})
    .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityServerConfiguration.GetClients())
    .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
    .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
    .AddProfileService<ProfileService>()
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAllOrigins");
app.UseRouting();

app.UseAuthorization();
app.UseIdentityServer();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    DataBuilderInitializer.Init(scope.ServiceProvider);
}
app.Run();
