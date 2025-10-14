namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class ChangePasswordRequest : BaseUserNameRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public bool? SendEmail { get; set; }
    }
}
