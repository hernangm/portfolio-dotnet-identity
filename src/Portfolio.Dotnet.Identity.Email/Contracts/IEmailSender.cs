using Portfolio.Dotnet.Identity.Email.Emails.AccountCreated;
using Portfolio.Dotnet.Identity.Email.Emails.PasswordChanged;
using Portfolio.Dotnet.Identity.Email.Emails.ResetPassword;

namespace Portfolio.Dotnet.Identity.Email.Contracts
{
    public interface IEmailSender
    {
        Task<SendEmailResponse> SendResetPasswordEmail(EmailRecipient to, ResetPasswordEmailModel model);
        Task<SendEmailResponse> SendPasswordChangedEmail(EmailRecipient to, PasswordChangedEmailModel model);
        Task<SendEmailResponse> SendAccountCreatedEmail(EmailRecipient to, AccountCreatedEmailModel model);
    }
}