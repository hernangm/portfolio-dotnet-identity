using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Text.RegularExpressions;

namespace Portfolio.Dotnet.Identity.Server.Infra
{
    public class RegexRedirectUriValidator : IRedirectUriValidator
    {
        protected bool StringCollectionContainsString(IEnumerable<string> uris, string requestedUri)
        {
            if (uris.IsNullOrEmpty())
            {
                return false;
            }
            return uris.Any(item => Regex.IsMatch(requestedUri, item, RegexOptions.IgnoreCase));
        }


        public virtual Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(StringCollectionContainsString(client.RedirectUris, requestedUri));
        }

        public virtual Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(StringCollectionContainsString(client.PostLogoutRedirectUris, requestedUri));
        }
    }
}
