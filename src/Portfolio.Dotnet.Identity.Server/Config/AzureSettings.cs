namespace Portfolio.Dotnet.Identity.Server.Config
{

    public class AzureSettings : BaseExternalAuthenticationProviderSettings
    {
        public string TenantId { get; set; } = string.Empty;
    }
}
