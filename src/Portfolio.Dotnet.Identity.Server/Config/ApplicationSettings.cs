using Portfolio.Dotnet.Identity.Email.Registration;
using Portfolio.Dotnet.Identity.Users.PasswordGenerator;

namespace Portfolio.Dotnet.Identity.Server.Config
{
    public class ApplicationSettings
    {
        public int? AccessTokenLifetimeInSeconds { get; set; }
        public int? IdentityTokenLifetimeInSeconds { get; set; }
        public string? IssuerUrl { get; set; }
        public ExternalIdentityProvidersSettings ExternalIdentityProviders { get; set; } = null!;
        public EmailSettings Email { get; set; } = null!;
        public string Environment { get; set; } = string.Empty;
        public PasswordGeneratorPolicySettings PasswordPolicy { get; set; } = null!;
        public int? SlidingRefreshTokenLifetimeInSeconds { get; set; }
        public UISettings UI { get; set; } = null!;

        public bool IsProduction()
        {
            return Environment.Equals("production", StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsStaging()
        {
            return Environment?.ToLower() == "staging";
        }
    }
}
