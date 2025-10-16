using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Consent;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Consent
{
    [TestClass]
    public class ConsentControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockInteractionService;
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<ILogger<ConsentController>> _mockLogger;
        private readonly ConsentController _controller;

        public ConsentControllerTests()
        {
            _mockInteractionService = new Mock<IIdentityServerInteractionService>();
            _mockEventService = new Mock<IEventService>();
            _mockLogger = new Mock<ILogger<ConsentController>>();

            _controller = new ConsentController(
                _mockInteractionService.Object,
                _mockEventService.Object,
                _mockLogger.Object);
        }

        [TestMethod]
        public async Task Index_Get_ReturnsViewResult()
        {
            // Arrange
            var returnUrl = "http://localhost:5000";

            // Act
            var result = await _controller.Index(returnUrl);

            // Assert
            Assert.IsInstanceOfType<ViewResult>(result);
        }
    }
}
