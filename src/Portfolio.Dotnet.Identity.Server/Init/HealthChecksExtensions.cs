using Portfolio.Dotnet.Identity.Server.Config;
using Portfolio.Dotnet.Identity.Users.Data;

namespace Portfolio.Dotnet.Identity.Server.Init
{
    public static class HealthChecksBuilderExtensions
    {
        public static void RegisterHealthChecks(this IHealthChecksBuilder healthChecksBuilder, ApplicationSettings applicationSettings)
        {
            healthChecksBuilder.AddDbContextCheck<ThisIdentityDbContext>();
            if (applicationSettings.Email.Enabled)
            {
                //healthChecksBuilder.AddSmtpHealthCheck(options =>
                //    {
                //        options.Host = applicationSettings.Email.SMTP.Host;
                //        options.Port = applicationSettings.Email.SMTP.Port;
                //        // Add other options like SSL/TLS if needed
                //    }, name: "SMTP");
            }
        }
    }
}
