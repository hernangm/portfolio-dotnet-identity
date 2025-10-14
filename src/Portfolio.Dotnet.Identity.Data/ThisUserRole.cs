using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data
{
    public class ThisUserRole : IdentityUserRole<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}