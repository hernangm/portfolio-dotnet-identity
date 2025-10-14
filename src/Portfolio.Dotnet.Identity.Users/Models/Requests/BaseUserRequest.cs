using System.Security.Claims;

namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public abstract class BaseUserRequest : BaseUserNameRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public List<Claim> Claims { get; set; } = [];


        public void AddClaim(string type, string value)
        {
            Claims.Add(new Claim(type, value));
        }

    }
}
