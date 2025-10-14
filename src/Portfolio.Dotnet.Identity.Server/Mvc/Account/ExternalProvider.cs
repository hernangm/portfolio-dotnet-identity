namespace Portfolio.Dotnet.Identity.Server.Mvc.Account
{
    public class ExternalProvider
    {
        public string? DisplayName { get; set; }
        public string AuthenticationScheme { get; set; } = string.Empty;
    }
}