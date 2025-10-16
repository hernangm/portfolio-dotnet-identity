using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Dotnet.Identity.Data.Migrations.Migrations
{
    public class SeedDataMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var client = new IdentityServer4.Models.Client
            {
                ClientId = 1.ToString(),
                ClientName = "portfolio-dotnet-abstracts",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = new List<string> {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false,
                AllowRememberConsent = false,
                RequirePkce = false,
                RedirectUris = ["http://localhost:8080/signin-oidc"],
                PostLogoutRedirectUris = ["http://localhost:8080/signout-callback-oidc"]
            };

            migrationBuilder.InsertData("Status",
                columns: new[] {
                    nameof(Client.ClientId),
                    nameof(Client.ClientName),
                    nameof(Client.AllowAccessTokensViaBrowser),
                    nameof(Client.AlwaysIncludeUserClaimsInIdToken),
                    nameof(Client.RequireConsent),
                    nameof(Client.AllowRememberConsent),
                },
                values: new object[] {
                    client.ClientId,
                    client.ClientName,
                    client.AllowAccessTokensViaBrowser,
                    client.AlwaysIncludeUserClaimsInIdToken,
                    client.RequireConsent,
                    client.AllowRememberConsent,
                    client.RequirePkce
                });
        }
    }
}
