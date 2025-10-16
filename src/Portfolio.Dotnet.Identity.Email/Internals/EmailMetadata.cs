namespace Portfolio.Dotnet.Identity.Email.Internals
{
    internal class EmailMetadata(string subject, string filePath)
    {
        public string Subject { get; set; } = subject;
        public string FilePath { get; set; } = filePath;
    }
}
