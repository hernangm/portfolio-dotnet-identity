using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Users.Data
{
    public class ThisUserClaim : IdentityUserClaim<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}
