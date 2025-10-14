namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class UpdateUserRequest : BaseUserRequest
    {
        public string? PreviousUsername { get; set; }
    }
}
