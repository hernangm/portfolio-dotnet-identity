using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Grants;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Grants
{
    [TestClass]
    public class GrantsControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockInteractionService;
        private readonly Mock<IClientStore> _mockClientStore;
        private readonly Mock<IResourceStore> _mockResourceStore;
        private readonly Mock<IEventService> _mockEventService;
        private readonly GrantsController _controller;

        public GrantsControllerTests()
        {
            _mockInteractionService = new Mock<IIdentityServerInteractionService>();
            _mockClientStore = new Mock<IClientStore>();
            _mockResourceStore = new Mock<IResourceStore>();
            _mockEventService = new Mock<IEventService>();

            _controller = new GrantsController(
                _mockInteractionService.Object,
                _mockClientStore.Object,
                _mockResourceStore.Object,
                _mockEventService.Object);
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
