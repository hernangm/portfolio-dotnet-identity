namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class UpdateClaimRequest : BaseUserNameRequest
    {
        public UpdateClaimRequest(string userName, string claimType, string claimValue)
        {
            UserName = userName;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
