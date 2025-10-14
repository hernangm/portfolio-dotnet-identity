﻿namespace Portfolio.Dotnet.Identity.Server.Init
{
    internal static class ApplicationSettingsRegistrationExtensions
    {
        public static TApplicationSettings RegisterApplicationSettings<TApplicationSettings>(this IServiceCollection services, IConfiguration configuration, string? key = null) where TApplicationSettings : class
        {
            var _key = key ?? typeof(TApplicationSettings).Name;
            var applicationSettings = Activator.CreateInstance<TApplicationSettings>();
            configuration.GetSection(_key).Bind(applicationSettings);
            services.AddSingleton(applicationSettings);
            return applicationSettings;
        }
    }
}
