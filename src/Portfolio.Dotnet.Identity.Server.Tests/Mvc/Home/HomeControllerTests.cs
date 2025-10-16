using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Home;
using Portfolio.Dotnet.Identity.Server.Mvc.Home.Models;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Home
{
    [TestClass]
    public class HomeControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockInteractionService;
        private readonly Mock<IWebHostEnvironment> _mockHostingEnvironment;
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockInteractionService = new Mock<IIdentityServerInteractionService>();
            _mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            _mockLogger = new Mock<ILogger<HomeController>>();

            _controller = new HomeController(
                _mockInteractionService.Object,
                _mockHostingEnvironment.Object,
                _mockLogger.Object);
        }

        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType<ViewResult>(result);
        }

        [TestMethod]
        public async Task Error_WithValidErrorId_ReturnsViewWithModel()
        {
            // Arrange
            var errorId = "test_error_id";
            var errorMessage = new ErrorMessage { Error = "test_error", ErrorDescription = "Test error description" };
            _mockInteractionService.Setup(i => i.GetErrorContextAsync(errorId)).ReturnsAsync(errorMessage);
            _mockHostingEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Production);

            // Act
            var result = await _controller.Error(errorId);

            // Assert
            var viewResult = Assert.IsInstanceOfType<ViewResult>(result);
            var model = Assert.IsInstanceOfType<ErrorViewModel>(viewResult.Model);
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Error);
            Assert.AreEqual(errorMessage.Error, model.Error.Error);
        }

        [TestMethod]
        public async Task Error_WithInvalidErrorId_ReturnsViewWithModel()
        {
            // Arrange
            var errorId = "invalid_error_id";
            _mockInteractionService.Setup(i => i.GetErrorContextAsync(errorId)).ReturnsAsync((ErrorMessage?)null);
            _mockHostingEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Production);

            // Act
            var result = await _controller.Error(errorId);

            // Assert
            var viewResult = Assert.IsInstanceOfType<ViewResult>(result);
            var model = Assert.IsInstanceOfType<ErrorViewModel>(viewResult.Model);
            Assert.IsNotNull(model);
            Assert.IsNull(model.Error);
        }

        [TestMethod]
        public async Task Error_InDevelopment_ReturnsViewWithModel()
        {
            // Arrange
            var errorId = "dev_error_id";
            var errorMessage = new ErrorMessage { Error = "dev_error", ErrorDescription = "Dev error description" };
            _mockInteractionService.Setup(i => i.GetErrorContextAsync(errorId)).ReturnsAsync(errorMessage);
            _mockHostingEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

            // Act
            var result = await _controller.Error(errorId);

            // Assert
            var viewResult = Assert.IsInstanceOfType<ViewResult>(result);
            var model = Assert.IsInstanceOfType<ErrorViewModel>(viewResult.Model);
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Error);
            Assert.AreEqual(errorMessage.Error, model.Error.Error);
        }
    }
}
