using FluentEmail.Core;
using Portfolio.Dotnet.Identity.Email.Emails.AccountCreated;
using Portfolio.Dotnet.Identity.Email.Emails.PasswordChanged;
using Portfolio.Dotnet.Identity.Email.Emails.ResetPassword;

namespace Portfolio.Dotnet.Identity.Email.Contracts
{
    public interface IEmailFactory
    {
        IFluentEmail CreateAccountCreatedEmail(EmailRecipient to, AccountCreatedEmailModel model);
        IFluentEmail CreatePasswordChangedEmail(EmailRecipient to, PasswordChangedEmailModel model);
        IFluentEmail CreateResetPasswordEmail(EmailRecipient to, ResetPasswordEmailModel model);
    }
}
