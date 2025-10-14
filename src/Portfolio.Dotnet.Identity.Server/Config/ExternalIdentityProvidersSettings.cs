namespace Portfolio.Dotnet.Identity.Server.Config
{
    public class ExternalIdentityProvidersSettings
    {
        public AzureSettings Azure { get; set; } = null!;
        public GoogleSettings Google { get; set; } = null!;
        public bool PrioritizeManageUsersWhenLinking { get; set; } = false;
    }
}
