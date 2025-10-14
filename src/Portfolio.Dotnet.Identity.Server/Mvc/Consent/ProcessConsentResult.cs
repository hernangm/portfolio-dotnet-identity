using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Server.Mvc.Consent.Models;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Consent
{
    public class ProcessConsentResult
    {
        public bool IsRedirect => RedirectUri != null;
        public string? RedirectUri { get; set; }
        public Client Client { get; set; } = null!;

        public bool ShowView => ViewModel != null;
        public ConsentViewModel? ViewModel { get; set; }

        public bool HasValidationError => ValidationError != null;
        public string? ValidationError { get; set; }
    }
}
