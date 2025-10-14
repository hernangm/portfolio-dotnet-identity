namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class SendResetPasswordEmailRequest : BaseUserNameRequest
    {
        public string ResetPasswordUrl { get; set; } = string.Empty;
    }
}
