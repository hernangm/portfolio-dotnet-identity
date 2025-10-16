using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data.Users
{
    public class ThisRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ThisRole Role { get; set; } = null!;
    }
}
