namespace Portfolio.Dotnet.Identity.Email.Internals
{
    public class EmailMetadata(string subject, string filePath)
    {
        public string Subject { get; set; } = subject;
        public string FilePath { get; set; } = filePath;
    }
}
