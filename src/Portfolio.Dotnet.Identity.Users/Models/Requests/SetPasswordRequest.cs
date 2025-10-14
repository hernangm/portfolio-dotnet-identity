namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class SetPasswordRequest : BaseUserNameRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
