using Portfolio.Dotnet.Identity.Configuration.Utils;
using Portfolio.Dotnet.Identity.Server.Config;

namespace Portfolio.Dotnet.Identity.Server.Init
{
    public static class ApplicationRegistrationExtensions
    {
        public static void RegisterApplications(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            var defaultValue = 60 * 60 * 6;
            var scanResults = ApplicationScanner.Scan(
                new ClientConfiguration
                {
                    AccessTokenLifetimeInSeconds = applicationSettings.AccessTokenLifetimeInSeconds ?? defaultValue,
                    IdentityTokenLifetimeInSeconds = applicationSettings.IdentityTokenLifetimeInSeconds ?? defaultValue,
                    SlidingRefreshTokenLifetimeInSeconds = applicationSettings.SlidingRefreshTokenLifetimeInSeconds ?? defaultValue,
                    Secret = "qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo"
                },
                new ResourceConfiguration
                {
                    Secret = "qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo"
                });
            services.AddSingleton(scanResults);
        }
    }
}
