using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Portfolio.Dotnet.Identity.Server.Mvc.Account;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Data;

namespace Portfolio.Dotnet.Identity.Server.Tests.Mvc.Account
{
    [TestClass]
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IIdentityServerInteractionService> _mockInteractionService;
        private readonly Mock<IClientStore> _mockClientStore;
        private readonly Mock<IAuthenticationSchemeProvider> _mockSchemeProvider;
        private readonly Mock<IEventService> _mockEventService;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockInteractionService = new Mock<IIdentityServerInteractionService>();
            _mockClientStore = new Mock<IClientStore>();
            _mockSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            _mockEventService = new Mock<IEventService>();

            // 1. Create a mock for IUserStore
            var mockUserStore = new Mock<IUserStore<ThisUser>>();

            // 2. Instantiate UserManager with its mocked dependencies
            var _userManager = new UserManager<ThisUser>(
                mockUserStore.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ThisUser>>().Object,
                [],
                [],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ThisUser>>>().Object);

            // 3. Instantiate SignInManager with its mocked dependencies
            var _signInManager = new SignInManager<ThisUser>(
                _userManager,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ThisUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ThisUser>>>().Object,
                _mockSchemeProvider.Object,
                new Mock<IUserConfirmation<ThisUser>>().Object);

            _controller = new AccountController(
                _mockUserService.Object,
                _mockInteractionService.Object,
                _mockClientStore.Object,
                _mockSchemeProvider.Object,
                _mockEventService.Object,
                _signInManager);
        }

        [TestMethod]
        public async Task Login_Get_ReturnsViewResult()
        {
            // Arrange
            var returnUrl = "http://localhost:5000";

            // Act
            var result = await _controller.Login(returnUrl);

            // Assert
            Assert.IsInstanceOfType<ViewResult>(result);
        }
    }
}
