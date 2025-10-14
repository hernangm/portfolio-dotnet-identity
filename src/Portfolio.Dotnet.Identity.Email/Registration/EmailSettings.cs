namespace Portfolio.Dotnet.Identity.Email.Registration
{
    public class EmailSettings
    {
        public bool Enabled { get; set; }
        public string From { get; set; } = string.Empty;
        public string SendGridApiKey { get; set; } = string.Empty;
        public string TestRecipients { get; set; } = string.Empty;
    }
}
