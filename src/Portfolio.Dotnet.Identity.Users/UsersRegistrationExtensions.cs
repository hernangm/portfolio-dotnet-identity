using Microsoft.Extensions.DependencyInjection;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.PasswordGenerator;

namespace Portfolio.Dotnet.Identity.Users
{
    public static class UsersRegistrationExtensions
    {
        public static IServiceCollection RegisterUsersServices(this IServiceCollection services, PasswordGeneratorPolicySettings passwordPolicySettings)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordGeneratorService>(sp => new PasswordGeneratorService(passwordPolicySettings));
            return services;
        }
    }

}
