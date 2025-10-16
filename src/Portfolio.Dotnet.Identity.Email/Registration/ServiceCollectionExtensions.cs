using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Dotnet.Identity.Email.Contracts;

namespace Portfolio.Dotnet.Identity.Email.Registration
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterEmailServices(this IServiceCollection services, EmailSettings emailSettings, string basePath, string environment, bool isProduction)
        {
            var a = services.AddFluentEmail(emailSettings.From)
                .AddRazorRenderer(basePath)
                .AddSendGridSender(emailSettings.SendGridApiKey);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailFactory, EmailFactory>(sp => new EmailFactory(sp.GetRequiredService<IFluentEmail>(), new EmailFactoryConfiguration
            {
                BasePath = basePath,
                Environment = environment,
                IsProduction = isProduction,
                TestRecipients = emailSettings.TestRecipients
            }));
        }

    }
}
