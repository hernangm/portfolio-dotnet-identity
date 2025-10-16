using Microsoft.AspNetCore.Identity;
using Portfolio.Dotnet.Identity.Configuration.Utils;
using Portfolio.Dotnet.Identity.Users.Data;

namespace Portfolio.Dotnet.Identity.Configuration.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var scanResults = ApplicationScanner.Scan(new ClientConfiguration(), new ResourceConfiguration());
            Assert.IsNotNull(scanResults);
            Assert.IsTrue(scanResults.Clients.Any());
            Assert.IsTrue(scanResults.Resources.Any());
            Assert.IsTrue(scanResults.Scopes.Any());
        }


        [TestMethod]
        public void PasswordHasher_Should_Verify_Correct_Password()
        {
            // Arrange
            var passwordHasher = new PasswordHasher<ThisUser>();
            var user = new ThisUser();
            var password = "password12344";
            var wrongPassword = "password56789";

            // Act
            var hashedPassword = passwordHasher.HashPassword(user, password);

            // Assert
            // Verify that the correct password succeeds
            var correctPasswordResult = passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            Assert.AreEqual(PasswordVerificationResult.Success, correctPasswordResult);

            // Verify that an incorrect password fails
            var wrongPasswordResult = passwordHasher.VerifyHashedPassword(user, hashedPassword, wrongPassword);
            Assert.AreEqual(PasswordVerificationResult.Failed, wrongPasswordResult);
        }
    }
}
