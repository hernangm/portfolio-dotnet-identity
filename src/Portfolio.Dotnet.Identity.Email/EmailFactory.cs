using FluentEmail.Core;
using FluentEmail.Core.Models;
using Portfolio.Dotnet.Identity.Email.Contracts;
using Portfolio.Dotnet.Identity.Email.Emails.AccountCreated;
using Portfolio.Dotnet.Identity.Email.Emails.PasswordChanged;
using Portfolio.Dotnet.Identity.Email.Emails.ResetPassword;
using Portfolio.Dotnet.Identity.Email.Internals;

namespace Portfolio.Dotnet.Identity.Email
{
    public class EmailFactory(IFluentEmail fluentEmail, EmailFactoryConfiguration configuration) : IEmailFactory
    {
        private readonly IFluentEmail FluentEmail = fluentEmail;
        private readonly EmailFactoryConfiguration Configuration = configuration;

        public IFluentEmail CreateAccountCreatedEmail(EmailRecipient to, AccountCreatedEmailModel model)
        {
            return CreateEmail(to, EmailCatalog.AccountCreated, model);
        }

        public IFluentEmail CreatePasswordChangedEmail(EmailRecipient to, PasswordChangedEmailModel model)
        {
            return CreateEmail(to, EmailCatalog.PasswordChanged, model);
        }

        public IFluentEmail CreateResetPasswordEmail(EmailRecipient to, ResetPasswordEmailModel model)
        {
            return CreateEmail(to, EmailCatalog.ResetPassword, model);
        }

        private IFluentEmail CreateEmail<T>(EmailRecipient to, EmailMetadata emailMetadata, T model)
        {
            IFluentEmail email;
            if (!Configuration.IsProduction)
            {
                var addresses = Configuration.TestRecipients.Split(',', ';').Select(m => new Address(m));
                email = FluentEmail.To(addresses).Subject($"{Configuration.Environment}: {emailMetadata.Subject}").PlaintextAlternativeBody($"Original Recipients: {to.Email}");
            }
            else
            {
                email = FluentEmail.To(to.Email, to.Name).Subject(emailMetadata.Subject);
            }
            var fullPath = Path.Combine(Configuration.BasePath, emailMetadata.FilePath);
            return email.Tag($"{Guid.NewGuid()}").UsingTemplateFromFile(fullPath, model);
        }
    }
}
