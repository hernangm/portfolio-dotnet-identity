namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class ChangePhoneNumberRequest : BaseUserNameRequest
    {
        public string? PhoneNumber { get; set; }
    }
}