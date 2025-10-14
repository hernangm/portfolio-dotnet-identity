namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class SetPasswordWithTokenRequest : SetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
    }
}
