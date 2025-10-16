using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data.Users
{
    public class ThisUserRole : IdentityUserRole<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}
