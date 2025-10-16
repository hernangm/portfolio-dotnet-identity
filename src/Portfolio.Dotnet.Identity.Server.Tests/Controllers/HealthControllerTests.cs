using Microsoft.AspNetCore.Mvc;
using Portfolio.Dotnet.Identity.Server.Controllers;

namespace Portfolio.Dotnet.Identity.Server.Tests.Controllers
{
    [TestClass]
    public class HealthControllerTests
    {
        private readonly HealthController _controller;

        public HealthControllerTests()
        {
            _controller = new HealthController();
        }

        [TestMethod]
        public void Health_ReturnsOkResult()
        {
            // Arrange

            // Act
            var result = _controller.Health();

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }
    }
}
