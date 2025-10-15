namespace Portfolio.Dotnet.Identity.Configuration.Utils
{
    public class ClientConfiguration
    {
        public int SlidingRefreshTokenLifetimeInSeconds { get; set; }
        public int AccessTokenLifetimeInSeconds { get; set; }
        public int IdentityTokenLifetimeInSeconds { get; set; }
        public string Secret { get; set; } = string.Empty;
    }
}
