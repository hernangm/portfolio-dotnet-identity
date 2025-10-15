using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Portfolio.Dotnet.Identity.Users.Data;

namespace Portfolio.Dotnet.Identity.Users
{
    public sealed class ProfileService(
        UserManager<ThisUser> userManager,
        IUserClaimsPrincipalFactory<ThisUser> userClaimsPrincipalFactory) : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ThisUser> UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
        private readonly UserManager<ThisUser> UserManager = userManager;

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await UserManager.FindByIdAsync(sub);
            if (user != null)
            {
                var userClaims = await UserClaimsPrincipalFactory.CreateAsync(user);
                context.AddRequestedClaims(userClaims.Claims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await UserManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
