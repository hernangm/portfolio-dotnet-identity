using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Device;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Device
{
    [TestClass]
    public class DeviceControllerTests
    {
        private readonly Mock<IDeviceFlowInteractionService> _mockInteractionService;
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<IOptions<IdentityServerOptions>> _mockOptions;
        private readonly DeviceController _controller;

        public DeviceControllerTests()
        {
            _mockInteractionService = new Mock<IDeviceFlowInteractionService>();
            _mockEventService = new Mock<IEventService>();
            _mockOptions = new Mock<IOptions<IdentityServerOptions>>();
            _mockOptions.Setup(o => o.Value).Returns(new IdentityServerOptions());

            _controller = new DeviceController(
                _mockInteractionService.Object,
                _mockEventService.Object,
                _mockOptions.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [TestMethod]
        public async Task Index_Get_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType<ViewResult>(result);
        }
    }
}
