using Duende.IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Configuration.Utils;

namespace Portfolio.Dotnet.Identity.Configuration
{
    public class POCRegistration : BaseRegistration
    {
        public POCRegistration() : base("Proof of Concept")
        {
        }

        public override IEnumerable<Client> GetClients(ClientConfiguration configuration)
        {

            var clients = new List<Client>
            {
                CreateClient(Clients.ProofOfConcept
                    , ApplicationName
                    , GrantTypes.Implicit
                    , configuration
                    , [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Scopes.ProofOfConceptApi
                    ]
                    , null
                    , null
                    , (options) => {
                        options.AllowAccessTokensViaBrowser = true;
                        options.AlwaysIncludeUserClaimsInIdToken = true;
                        options.RequireConsent = false;
                        options.AllowRememberConsent = false;
                        options.RequirePkce = false;
                        options.RedirectUris = ["http://localhost:8080/signin-oidc"];
                        options.PostLogoutRedirectUris = ["http://localhost:8080/signout-callback-oidc"];
                    }
                )
            };
            return clients;
        }

        public override IEnumerable<ApiResource> GetResources(ResourceConfiguration configuration)
        {
            var resources = new List<ApiResource>
            {
                new(Scopes.ProofOfConceptApi, $"{ApplicationName} API")
                {
                    ApiSecrets =
                    {
                        new Secret(configuration.Secret.Sha256())
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Name
                    },
                    Scopes = [Scopes.ProofOfConceptApi]
                }
            };
            return resources;
        }

        public override IEnumerable<ApiScope> GetScopes()
        {
            var scopes = new List<ApiScope>
            {
                new(Scopes.ProofOfConceptApi, $"{ApplicationName} API")
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Name
                    }
                }
            };
            return scopes;
        }

        internal static class Clients
        {
            public static string ProofOfConcept => "proof-of-concept";
        }

        internal static class Scopes
        {
            public static string ProofOfConceptApi => "proof-of-concept-api";
        }
    }
}
