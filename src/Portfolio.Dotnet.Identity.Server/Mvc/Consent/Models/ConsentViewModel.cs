namespace Portfolio.Dotnet.Identity.Server.Mvc.Consent.Models
{
    public class ConsentViewModel : ConsentInputModel
    {
        public string ClientName { get; set; } = string.Empty;
        public string? ClientUrl { get; set; }
        public string? ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = [];
        public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = [];
    }
}
