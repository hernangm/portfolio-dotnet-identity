using Portfolio.Dotnet.Identity.Email.Contracts;
using Portfolio.Dotnet.Identity.Email.Emails.ResetPassword;
using Portfolio.Dotnet.Identity.Email.Internals;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Moq;

namespace Portfolio.Dotnet.Identity.Email.Tests
{
    [TestClass]
    public class EmailTests
    {

        [TestMethod]
        public async Task TestMethod1()
        {
            var mockFluentEmail = new Mock<IFluentEmail>();
            mockFluentEmail.Setup(m => m.To(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.Subject(It.IsAny<string>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.UsingTemplateFromFile(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<bool>())).Returns(mockFluentEmail.Object);
            mockFluentEmail.Setup(m => m.SendAsync(null)).ReturnsAsync(new SendResponse());

            var mockEmailFactory = new Mock<IEmailFactory>();
            var fluentEmailService = new EmailSender(mockEmailFactory.Object, null);

            var sendResponse = await fluentEmailService.SendResetPasswordEmail(new EmailRecipient(to), new ResetPasswordEmailModel { ResetPasswordLink = "12345" });

            Assert.IsNotNull(sendResponse);
            Assert.IsTrue(sendResponse.Successful);
            mockFluentEmail.Verify(f => f.To(to), Times.Once(), $"Recipient should be set as: '{to}'.");
            mockFluentEmail.Verify(f => f.Subject(EmailCatalog.ResetPassword.Subject), Times.Once, $"Subject should be set as: '{EmailCatalog.ResetPassword.Subject}'.");
            mockFluentEmail.Verify(f => f.SendAsync(null), Times.Once, "1 email should be sent.");
        }
    }
}