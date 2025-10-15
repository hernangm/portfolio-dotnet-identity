using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Globalization;

namespace Portfolio.Dotnet.Identity.Users.Data
{
    public class ThisUser : IdentityUser<int>
    {
        public ICollection<ThisUserClaim> UserClaims { get; set; } = [];
        public ICollection<ThisUserRole> UserRoles { get; set; } = null!;
        public ICollection<ThisUserLogin> UserLogins { get; set; } = null!;


        public ThisUserClaim? GetClaim(string claimType)
        {
            return UserClaims.SingleOrDefault(uc => uc.ClaimType == claimType);
        }

        public string? GetClaimValue(string claimType, string? defaultValue = null)
        {
            var userClaim = UserClaims.FirstOrDefault(uc => uc.ClaimType == claimType);
            return userClaim?.ClaimValue ?? defaultValue;
        }

        public TValue GetClaimValue<TValue>(string claimType, TValue defaultValue = default)
            where TValue : struct
        {
            var userClaim = UserClaims.SingleOrDefault(uc => uc.ClaimType == claimType);
            if (userClaim is null || string.IsNullOrEmpty(userClaim.ClaimValue))
            {
                return defaultValue;
            }
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TValue));
                var value = converter.ConvertFromString(null, CultureInfo.InvariantCulture, userClaim.ClaimValue);
                if (value is null)
                {
                    return defaultValue;
                }
                var resultValue = (TValue)value;
                return resultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
