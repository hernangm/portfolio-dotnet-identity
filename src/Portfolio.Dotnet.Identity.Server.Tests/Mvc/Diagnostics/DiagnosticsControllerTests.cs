using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Diagnostics;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Diagnostics
{
    [TestClass]
    public class DiagnosticsControllerTests
    {
        private readonly DiagnosticsController _controller;

        public DiagnosticsControllerTests()
        {
            _controller = new DiagnosticsController();
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType<ViewResult>(result);
        }
    }
}
