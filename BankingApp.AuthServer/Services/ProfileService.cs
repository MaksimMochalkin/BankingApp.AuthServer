namespace BankingApp.AuthServer.Services
{
    using IdentityServer4.Models;
    using IdentityServer4.Services;

    public class ProfileService : IProfileService
    {
        public ProfileService() 
        {
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
