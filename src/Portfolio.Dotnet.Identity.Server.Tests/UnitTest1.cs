using IdentityServer4.Models;
using Portfolio.Dotnet.Identity.Server.Infra;

namespace Portfolio.Dotnet.Identity.Server.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var client = CreateClient();
            var uriValidator = new RegexRedirectUriValidator();
            var uris = new List<Tuple<string, bool>>()
            {
                //new("https://localhost:44370/signin-oidc", true),

            };
            foreach (var uri in uris)
            {
                var r = await uriValidator.IsRedirectUriValidAsync(uri.Item1, client);
                Assert.AreEqual(uri.Item2, r);
            }
        }

        private static Client CreateClient()
        {

            var c = new Client
            {
                RedirectUris =
                [
                    //"https://localhost:44370/signin-oidc",
                ],
                PostLogoutRedirectUris =
                [
                    "https://localhost:44370"
                ]
            };

            return c;
        }
    }
}
