using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Configuration.Contracts;

namespace Portfolio.Dotnet.Identity.Configuration.Utils
{
    public static class ApplicationScanner
    {
        public static ApplicationScannerResults Scan(ClientConfiguration clientConfiguration, ResourceConfiguration resourceConfiguration)
        {
            var types = typeof(ApplicationScanner).Assembly.GetTypes().Where(m => m.IsPublic && !m.IsAbstract && typeof(IApplicationRegistration).IsAssignableFrom(m)).ToList();
            var clients = new List<Client>();
            var resources = new List<ApiResource>();
            var scopes = new List<ApiScope>();
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is IApplicationRegistration obj)
                {
                    AddToList(obj.GetClients(clientConfiguration), clients);
                    AddToList(obj.GetResources(resourceConfiguration), resources);
                    AddToList(obj.GetScopes(), scopes);
                }
            }
            return new ApplicationScannerResults
            {
                Clients = clients,
                Resources = resources,
                Scopes = scopes
            };
        }

        private static void AddToList<TObject>(IEnumerable<TObject>? source, List<TObject> destination)
        {
            if (source == null || !source.Any())
            {
                return;
            }
            destination.AddRange(source);
        }
    }
}
