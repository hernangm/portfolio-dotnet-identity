using PasswordGenerator;
using Portfolio.Dotnet.Identity.Users.Contracts;

namespace Portfolio.Dotnet.Identity.Users.PasswordGenerator
{
    public class PasswordGeneratorService(PasswordGeneratorPolicySettings settings) : IPasswordGeneratorService
    {
        private readonly PasswordGeneratorPolicySettings Settings = settings;

        public string GeneratePassword()
        {
            var psw = new Password(Settings.RequireLowercase
                , Settings.RequireUppercase
                , Settings.RequireDigit
                , Settings.RequireNonAlphanumeric
                , Settings.RequiredLength);
            return psw.Next();
        }
    }
}
