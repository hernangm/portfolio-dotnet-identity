using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data
{
    public class ThisRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ThisRole Role { get; set; } = null!;
    }
}