using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Configuration.Contracts;
using Portfolio.Dotnet.Identity.Configuration.Utils;
using System.Collections.ObjectModel;

namespace Portfolio.Dotnet.Identity.Configuration
{
    public abstract class BaseRegistration(string applicationName) : IApplicationRegistration
    {
        protected string ApplicationName { get; private set; } = applicationName;

        public abstract IEnumerable<Client> GetClients(ClientConfiguration configuration);
        public abstract IEnumerable<ApiResource> GetResources(ResourceConfiguration configuration);
        public abstract IEnumerable<ApiScope> GetScopes();

        protected static Client CreateClient(string clientId
            , string clientName
            , ICollection<string> allowedGrantTypes
            , ClientConfiguration configuration
            , IEnumerable<string>? scopes = null
            , IEnumerable<string>? origins = null
            , IDictionary<string, string>? properties = null
            , Action<Client>? configure = null
            , string? clientCustomSecret = null)
        {
            properties ??= new Dictionary<string, string>();
            var client = new Client
            {
                ClientId = clientId,
                ClientName = clientName,
                AllowedGrantTypes = allowedGrantTypes,
                AccessTokenType = AccessTokenType.Jwt,
                Enabled = true,
                ClientClaimsPrefix = string.Empty,
                ClientSecrets =
                [
                    new(!string.IsNullOrEmpty(clientCustomSecret?.Trim()) ? clientCustomSecret.Sha256() : configuration.Secret.Sha256())
                ],
                AllowedScopes = scopes?.ToArray(),
                AllowOfflineAccess = true,
                SlidingRefreshTokenLifetime = configuration.SlidingRefreshTokenLifetimeInSeconds,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                UpdateAccessTokenClaimsOnRefresh = false,
                AccessTokenLifetime = configuration.AccessTokenLifetimeInSeconds,
                IdentityTokenLifetime = configuration.IdentityTokenLifetimeInSeconds,
                AllowedCorsOrigins = origins?.ToArray(),
                Properties = properties
            };
            configure?.Invoke(client);
            return client;
        }

        protected static ICollection<string> GetClaims(params IEnumerable<string>[] p)
        {
            var claims = new Collection<string>();
            foreach (var l in p)
            {
                if (l != null && l.Any())
                {
                    foreach (var e in l)
                    {
                        claims.Add(e);
                    }
                }
            }
            return claims;
        }
    }
}
