using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Users.Data
{
    public class ThisUserRole : IdentityUserRole<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}