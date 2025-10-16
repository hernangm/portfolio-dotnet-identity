using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Diagnostics;
using System.Net;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Diagnostics
{
    [TestClass]
    public class DiagnosticsControllerTests
    {
        private readonly DiagnosticsController _controller;

        public DiagnosticsControllerTests()
        {
            _controller = new DiagnosticsController();

            var mockAuthService = new Mock<IAuthenticationService>();
            mockAuthService
                .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .ReturnsAsync(AuthenticateResult.NoResult());

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(mockAuthService.Object);

            var httpContext = new DefaultHttpContext();

            var tempDataFactory = new Mock<ITempDataDictionaryFactory>();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempDataFactory.Setup(f => f.GetTempData(It.IsAny<HttpContext>())).Returns(tempData);

            mockServiceProvider
                .Setup(s => s.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(tempDataFactory.Object);

            httpContext.RequestServices = mockServiceProvider.Object;
            httpContext.Connection.RemoteIpAddress = IPAddress.Loopback;
            httpContext.Connection.LocalIpAddress = IPAddress.Loopback;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
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
