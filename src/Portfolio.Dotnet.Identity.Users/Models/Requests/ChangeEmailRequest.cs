namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class ChangeEmailRequest : BaseUserNameRequest
    {
        public string? Email { get; set; }
    }
}
