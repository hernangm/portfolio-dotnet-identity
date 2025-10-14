namespace Portfolio.Dotnet.Identity.Server.Config
{
    public abstract class BaseExternalAuthenticationProviderSettings
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
