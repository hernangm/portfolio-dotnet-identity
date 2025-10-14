using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data
{
    public class ThisUserLogin : IdentityUserLogin<int>
    {
        public virtual ThisUser User { get; set; } = null!;
    }
}