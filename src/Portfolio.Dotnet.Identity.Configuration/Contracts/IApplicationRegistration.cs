using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Configuration.Utils;

namespace Portfolio.Dotnet.Identity.Configuration.Contracts
{
    public interface IApplicationRegistration
    {
        IEnumerable<Client> GetClients(ClientConfiguration configuration);
        IEnumerable<ApiResource> GetResources(ResourceConfiguration configuration);
        IEnumerable<ApiScope> GetScopes();
    }
}
