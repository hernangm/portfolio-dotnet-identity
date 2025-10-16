using Duende.IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Configuration.Utils;

namespace Portfolio.Dotnet.Identity.Configuration
{
    public class POCRegistration : BaseRegistration
    {
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
    }
}
