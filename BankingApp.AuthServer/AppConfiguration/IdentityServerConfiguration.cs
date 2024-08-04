namespace BankingApp.AuthServer.AppConfiguration
{
    using IdentityServer4.Models;

    public static class IdentityServerConfiguration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("BankingApi.BankingApi", "BankingApi"),
            };

        public static IEnumerable<Client> GetClients()
        {
            var clientManager = new ClientManager();
            var clients = new List<Client>
            {
                clientManager.GetBankingApiAppSettings(),
            };
            return clients;
        }

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("BankingApi.BankingApi"),
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }
}
