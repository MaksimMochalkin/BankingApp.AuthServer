namespace BankingApp.AuthServer.AppConfiguration
{
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Models;

    public class ClientManager
    {
        public Client GetBankingApiAppSettings()
        {
            return new Client
            {
                ClientId = "BankingApiClient_id",
                ClientSecrets = new[] { new Secret("BankingApiClient_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                    {
                        "BankingApi.BankingApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                RedirectUris =
                {
                    "https://localhost:44334/swagger/oauth2-redirect.html",
                    "https://localhost:44334/swagger/index.html",
                    "https://localhost:44334/signin-oidc"
                },
                RequireConsent = false,
                PostLogoutRedirectUris = { "https://localhost:44334/swagger/index.html" },
                RequirePkce = true,
                AccessTokenLifetime = 180,
                AllowOfflineAccess = true,
                //AllowedCorsOrigins = { "https://localhost:44334" } // можно настроить политику cors для каждого клиента
            };
        }
    }
}
