using IdentityServer4.Models;

namespace Portfolio.Dotnet.Identity.Data.Migrations
{
    public static class IdentityResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return
            [
                new IdentityServer4.Models.IdentityResources.OpenId(),
                new IdentityServer4.Models.IdentityResources.Profile(),
                new IdentityServer4.Models.IdentityResources.Email(),
            ];
        }
    }
}
