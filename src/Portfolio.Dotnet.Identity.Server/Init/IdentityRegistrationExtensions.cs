using Duende.IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.Dotnet.Identity.Server.Config;
using Portfolio.Dotnet.Identity.Server.Infra;
using Portfolio.Dotnet.Identity.Users;
using Portfolio.Dotnet.Identity.Users.Data;

namespace Portfolio.Dotnet.Identity.Server.Init
{
    public static class IdentityRegistrationExtensions
    {
        public static void RegisterIdentity(this IServiceCollection services, ApplicationSettings applicationSettings, Action<IServiceProvider, DbContextOptionsBuilder> resolveDbContextOptions)
        {
            services.AddDbContext<ThisIdentityDbContext>(resolveDbContextOptions);
            services.AddIdentity<ThisUser, ThisRole>(o =>
            {
                o.Password.RequireDigit = applicationSettings.PasswordPolicy.RequireDigit;
                o.Password.RequiredLength = applicationSettings.PasswordPolicy.RequiredLength;
                o.Password.RequiredUniqueChars = applicationSettings.PasswordPolicy.RequiredUniqueChars;
                o.Password.RequireLowercase = applicationSettings.PasswordPolicy.RequireLowercase;
                o.Password.RequireNonAlphanumeric = applicationSettings.PasswordPolicy.RequireNonAlphanumeric;
                o.Password.RequireUppercase = applicationSettings.PasswordPolicy.RequireUppercase;
            })
            .AddEntityFrameworkStores<ThisIdentityDbContext>()
            .AddDefaultTokenProviders();
            services.RegisterUsersServices(applicationSettings.PasswordPolicy);
        }

        public static void RegisterIdentityServer(this IServiceCollection services, string applicationSettingsIssuerUrl, Action<IServiceProvider, DbContextOptionsBuilder> resolveDbContextOptions)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });
            services.AddIdentityServer(options =>
            {
                options.IssuerUri = applicationSettingsIssuerUrl;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
                options.AccessTokenJwtType = "JWT";
            })
                .AddConfigurationStore(options =>
                {
                    options.ResolveDbContextOptions = resolveDbContextOptions;
                })
                .AddOperationalStore(options =>
                {
                    options.ResolveDbContextOptions = resolveDbContextOptions;
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ThisUser>()
                .AddProfileService<ProfileService>()
                .AddRedirectUriValidator<RegexRedirectUriValidator>();

        }
    }
}
