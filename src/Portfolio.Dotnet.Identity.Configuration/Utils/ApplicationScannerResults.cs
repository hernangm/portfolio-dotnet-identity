using IdentityServer4.Models;

namespace Portfolio.Dotnet.Identity.Configuration.Utils
{
    public class ApplicationScannerResults
    {
        public IEnumerable<Client> Clients { get; set; } = [];
        public IEnumerable<ApiResource> Resources { get; set; } = [];
        public IEnumerable<ApiScope> Scopes { get; set; } = [];
    }
}
