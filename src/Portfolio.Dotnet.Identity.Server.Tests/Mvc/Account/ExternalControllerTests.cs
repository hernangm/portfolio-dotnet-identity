using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Account;
using Portfolio.Dotnet.Identity.Users.Contracts;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Account
{
    [TestClass]
    public class ExternalControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockInteractionService;
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IClientStore> _mockClientStore;
        private readonly Mock<ILogger<ExternalController>> _mockLogger;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly ExternalController _controller;

        public ExternalControllerTests()
        {
            _mockInteractionService = new Mock<IIdentityServerInteractionService>();
            _mockEventService = new Mock<IEventService>();
            _mockUserService = new Mock<IUserService>();
            _mockClientStore = new Mock<IClientStore>();
            _mockLogger = new Mock<ILogger<ExternalController>>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ExternalController(
                _mockInteractionService.Object,
                _mockClientStore.Object,
                _mockEventService.Object,
                _mockLogger.Object,
                _mockEnvironment.Object,
                _mockUserService.Object);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/callback");
            _controller.Url = mockUrlHelper.Object;
        }

        [TestMethod]
        public void Challenge_ReturnsChallengeResult()
        {
            // Arrange
            var scheme = "Google";
            var returnUrl = "http://localhost:5000";
            _mockInteractionService.Setup(x => x.IsValidReturnUrl(returnUrl)).Returns(true);

            // Act
            var result = _controller.Challenge(scheme, returnUrl);

            // Assert
            Assert.IsInstanceOfType<ChallengeResult>(result);
        }
    }
}
