namespace Portfolio.Dotnet.Identity.Email
{
    public class EmailFactoryConfiguration
    {
        public string Environment { get; set; } = string.Empty;
        public bool IsProduction { get; set; } = true;
        public string BasePath { get; set; } = string.Empty;
        public string TestRecipients { get; set; } = string.Empty;
    }
}
