namespace BankingApp.AuthServer.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class IdentityServerAppContext : IdentityDbContext
    {
        public IdentityServerAppContext(DbContextOptions<IdentityServerAppContext> options)
            : base(options)
        {
        }
    }
}
