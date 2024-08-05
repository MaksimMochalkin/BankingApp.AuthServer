namespace RestaurantBooking.AuthServer.Data
{
    using BankingApp.AuthServer.AppConfiguration;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Security.Claims;

    public class DataBuilderInitializer
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            RoleManager.SeedRoles(roleManager).GetAwaiter().GetResult(); ;

            var user = new IdentityUser
            {
                UserName = "User@example.com",
                Email = "User@example.com",
            };

            var result = userManager.CreateAsync(user, "123qweAqws!!").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                var role = roleManager.FindByNameAsync("Administrator").GetAwaiter().GetResult();
                var claim = roleManager.GetClaimsAsync(role).GetAwaiter().GetResult().FirstOrDefault();
                userManager.AddClaimAsync(user, claim).GetAwaiter().GetResult();
            }

            //serviceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            //var context = serviceProvider.GetRequiredService<ConfigurationDbContext>();
            //context.Database.Migrate();
            //if (!context.Clients.Any())
            //{
            //    foreach (var client in IdentityServerConfiguration.GetClients())
            //    {
            //        context.Clients.Add(client.ToEntity());
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.IdentityResources.Any())
            //{
            //    foreach (var resource in IdentityServerConfiguration.IdentityResources)
            //    {
            //        context.IdentityResources.Add(resource.ToEntity());
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.ApiScopes.Any())
            //{
            //    foreach (var resource in IdentityServerConfiguration.ApiScopes)
            //    {
            //        context.ApiScopes.Add(resource.ToEntity());
            //    }
            //    context.SaveChanges();
            //}

            //if (!context.ApiResources.Any())
            //{
            //    foreach (var resource in IdentityServerConfiguration.ApiResources)
            //    {
            //        context.ApiResources.Add(resource.ToEntity());
            //    }
            //    context.SaveChanges();
            //}
        }
    }
}
