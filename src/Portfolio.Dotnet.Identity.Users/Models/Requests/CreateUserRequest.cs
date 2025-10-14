namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class CreateUserRequest : BaseUserRequest
    {
        public string? Password { get; set; }
        public string? ResetPasswordUrl { get; set; }
        public bool CreateResetAccountToken { get; set; }
        public bool? SendEmail { get; set; }
    }
}
