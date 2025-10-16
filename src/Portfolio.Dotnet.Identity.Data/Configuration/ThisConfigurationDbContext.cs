using Duende.IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Dotnet.Identity.Data.Configuration
{
    public class ThisConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) : ConfigurationDbContext(options, storeOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.IdentityResource>().HasData(
                new IdentityResources.OpenId().ToEntity(),
                new IdentityResources.Profile().ToEntity(),
                new IdentityResources.Email().ToEntity()
            );

            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.Client>().HasData(
                new Client
                {
                    ClientId = 1.ToString(),
                    ClientName = "portfolio-dotnet-abstracts",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = [
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile
                    ],
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    AllowRememberConsent = false,
                    RequirePkce = false,
                    RedirectUris = ["http://localhost:8080/signin-oidc"],
                    PostLogoutRedirectUris = ["http://localhost:8080/signout-callback-oidc"]
                }.ToEntity()
            );
            var apiResourceScope = "portfolio-dotnet-abstracts-api";
            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.ApiResource>().HasData(
                new ApiResource(apiResourceScope, [JwtClaimTypes.Email, JwtClaimTypes.Name])
                {
                    ApiSecrets =
                    {
                        new Secret()
                    },
                    Scopes = [apiResourceScope]

                }.ToEntity()
            );

            modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.ApiScope>().HasData(
                new ApiScope(apiResourceScope, [JwtClaimTypes.Email, JwtClaimTypes.Name]).ToEntity()
            );
        }
    }
}

