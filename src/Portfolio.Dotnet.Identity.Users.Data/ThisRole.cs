using Microsoft.AspNetCore.Identity;

namespace Portfolio.Dotnet.Identity.Users.Data
{
    public class ThisRole : IdentityRole<int>
    {
        public ICollection<ThisRoleClaim> RoleClaims { get; set; } = [];
    }
}
