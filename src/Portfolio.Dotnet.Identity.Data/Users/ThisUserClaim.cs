using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data.Users
{
    public class ThisUserClaim : IdentityUserClaim<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}
