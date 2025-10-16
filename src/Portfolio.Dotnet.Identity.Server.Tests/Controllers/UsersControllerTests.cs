using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.Dotnet.Identity.Server.Controllers;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Models;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;

namespace Portfolio.Dotnet.Identity.Server.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IPasswordGeneratorService> _mockPasswordGeneratorService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockPasswordGeneratorService = new Mock<IPasswordGeneratorService>();
            _controller = new UsersController(_mockUserService.Object, _mockPasswordGeneratorService.Object);
        }

        [TestMethod]
        public async Task GetUsers_ReturnsOkResult()
        {
            // Arrange
            _mockUserService.Setup(s => s.GetUsers()).ReturnsAsync([]);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result.Result);
        }

        [TestMethod]
        public async Task GetUserByUserName_UserExists_ReturnsUser()
        {
            // Arrange
            var userName = "test";
            var user = new UserDTO { UserName = userName };
            _mockUserService.Setup(s => s.GetUserByUserName(userName)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserByUserName(userName);

            // Assert
            Assert.IsInstanceOfType<UserDTO>(result.Value);
            Assert.AreEqual(userName, result.Value?.UserName);
        }

        [TestMethod]
        public async Task GetUserByUserName_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var userName = "test";
            _mockUserService.Setup(s => s.GetUserByUserName(userName)).ReturnsAsync((UserDTO?)null);

            // Act
            var result = await _controller.GetUserByUserName(userName);

            // Assert
            Assert.IsInstanceOfType<NotFoundResult>(result.Result);
        }

        [TestMethod]
        public async Task AddOrUpdateClaims_Success_ReturnsOk()
        {
            // Arrange
            var userName = "test";
            var request = new AddOrUpdateClaimsRequest();
            var response = new UsersOperationResponse(true, string.Empty);
            _mockUserService.Setup(s => s.AddOrUpdateClaims(It.IsAny<AddOrUpdateClaimsRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.AddOrUpdateClaims(userName, request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result.Result);
        }

        [TestMethod]
        public async Task AddOrUpdateClaims_Failure_ReturnsBadRequest()
        {
            // Arrange
            var userName = "test";
            var request = new AddOrUpdateClaimsRequest();
            var response = new UsersOperationResponse(false, "Error");
            _mockUserService.Setup(s => s.AddOrUpdateClaims(It.IsAny<AddOrUpdateClaimsRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.AddOrUpdateClaims(userName, request);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result.Result);
        }

        [TestMethod]
        public async Task RemoveClaims_Success_ReturnsOk()
        {
            // Arrange
            var userName = "test";
            var request = new RemoveClaimsRequest();
            var response = new UsersOperationResponse(true, string.Empty);
            _mockUserService.Setup(s => s.RemoveClaims(It.IsAny<RemoveClaimsRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.RemoveClaims(userName, request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result.Result);
        }

        [TestMethod]
        public async Task RemoveClaims_Failure_ReturnsBadRequest()
        {
            // Arrange
            var userName = "test";
            var request = new RemoveClaimsRequest();
            var response = new UsersOperationResponse(false, "Error");
            _mockUserService.Setup(s => s.RemoveClaims(It.IsAny<RemoveClaimsRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.RemoveClaims(userName, request);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result.Result);
        }

        [TestMethod]
        public void UserNameExists_ReturnsBool()
        {
            // Arrange
            var userName = "test";
            _mockUserService.Setup(s => s.UserNameExists(userName)).Returns(true);

            // Act
            var result = _controller.UserNameExists(userName);

            // Assert
            Assert.IsTrue(result.Value);
        }

        [TestMethod]
        public void GeneratePassword_ReturnsPassword()
        {
            // Arrange
            var password = "password";
            _mockPasswordGeneratorService.Setup(s => s.GeneratePassword()).Returns(password);

            // Act
            var result = _controller.GeneratePassword();

            // Assert
            Assert.IsInstanceOfType<GeneratePasswordResponse>(result.Value);
            Assert.AreEqual(password, result.Value?.Password);
        }
    }
}
