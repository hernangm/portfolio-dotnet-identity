using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Users.Data
{
    public class ThisRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ThisRole Role { get; set; } = null!;
    }
}