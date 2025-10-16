using Duende.IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.Dotnet.Identity.Data.Users;
using Portfolio.Dotnet.Identity.Server.Config;
using Portfolio.Dotnet.Identity.Server.Infra;
using Portfolio.Dotnet.Identity.Users;

namespace Portfolio.Dotnet.Identity.Server.Init
{
    public static class IdentityRegistrationExtensions
    {
        public static void RegisterIdentityServer(this IServiceCollection services, ApplicationSettings applicationSettings, Action<DbContextOptionsBuilder> configureDbContextOptions)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            services.AddScoped(sp =>
            {
                return new DbContextOptions<ConfigurationDbContext>();
            });
            services.AddDbContext<ThisIdentityDbContext>(configureDbContextOptions);

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

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = applicationSettings.IssuerUrl;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
                options.AccessTokenJwtType = "JWT";
            })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = configureDbContextOptions;
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = configureDbContextOptions;
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ThisUser>()
                .AddProfileService<ProfileService>()
                .AddRedirectUriValidator<RegexRedirectUriValidator>();
        }
    }
}
