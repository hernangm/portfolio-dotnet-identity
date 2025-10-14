using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Data
{
    public class ThisRole : IdentityRole<int>
    {
        public ICollection<ThisRoleClaim> RoleClaims { get; set; } = [];
    }
}