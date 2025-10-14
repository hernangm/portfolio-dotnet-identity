namespace Portfolio.Dotnet.Identity.Users.Models
{
    public class ClaimDTO
    {
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;

        public ClaimDTO() { }

        public ClaimDTO(string claimType, string claimValue)
            : base()
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
