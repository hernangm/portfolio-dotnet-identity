using Microsoft.AspNetCore.Identity;
using Portfolio.Dotnet.Identity.Configuration.Utils;
using Portfolio.Dotnet.Identity.Data;

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
        public void HashPassword()
        {
            var passwordHasher = new PasswordHasher<ThisUser>();
            var hashedPassword = passwordHasher.HashPassword(new ThisUser(), "password12344");
            Assert.AreEqual("AKMFMgLV1bVKsKT/Iu2Jq5SrcEbkNglUBpzZBH4GKW5Kf6tEjR75vrBJAYfNYhoI1Q==", hashedPassword);
        }
    }
}