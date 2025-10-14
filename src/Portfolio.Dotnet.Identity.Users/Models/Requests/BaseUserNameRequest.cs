namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public abstract class BaseUserNameRequest
    {
        public string UserName { get; set; } = string.Empty;
    }
}
