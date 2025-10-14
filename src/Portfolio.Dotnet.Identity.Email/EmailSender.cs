using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Portfolio.Dotnet.Identity.Email.Contracts;
using Portfolio.Dotnet.Identity.Email.Emails.AccountCreated;
using Portfolio.Dotnet.Identity.Email.Emails.PasswordChanged;
using Portfolio.Dotnet.Identity.Email.Emails.ResetPassword;

namespace Portfolio.Dotnet.Identity.Email
{
    public class EmailSender(IEmailFactory emailFactory, ILoggerFactory? loggerFactory) : IEmailSender
    {
        private readonly IEmailFactory EmailFactory = emailFactory;
        private readonly ILogger<EmailSender>? Logger = loggerFactory?.CreateLogger<EmailSender>();

        public Task<SendEmailResponse> SendAccountCreatedEmail(EmailRecipient to, AccountCreatedEmailModel model)
        {
            var email = EmailFactory.CreateAccountCreatedEmail(to, model);
            return SendEmail(email);
        }

        public Task<SendEmailResponse> SendPasswordChangedEmail(EmailRecipient to, PasswordChangedEmailModel model)
        {
            var email = EmailFactory.CreatePasswordChangedEmail(to, model);
            return SendEmail(email);
        }

        public Task<SendEmailResponse> SendResetPasswordEmail(EmailRecipient to, ResetPasswordEmailModel model)
        {
            var email = EmailFactory.CreateResetPasswordEmail(to, model);
            return SendEmail(email);
        }

        private async Task<SendEmailResponse> SendEmail(IFluentEmail email)
        {
            try
            {
                var sendResponse = await email.SendAsync();
                if (!sendResponse.Successful)
                {
                    throw new Exception($"Failed to send email: {string.Join(",", sendResponse.ErrorMessages)}");
                }
                return new SendEmailResponse
                {
                    MessageId = sendResponse.MessageId,
                    ErrorMessages = sendResponse.ErrorMessages
                };
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "{message}", ex.Message);
                return new SendEmailResponse() { ErrorMessages = [ex.Message] };
            }
        }
    }
}
