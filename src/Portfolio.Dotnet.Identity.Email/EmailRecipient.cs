namespace Portfolio.Dotnet.Identity.Email
{
    public class EmailRecipient(string email, string? name)
    {
        public string Email { get; private set; } = email;
        public string? Name { get; private set; } = name;

        public EmailRecipient(string email) : this(email, null)
        {
        }
    }
}
