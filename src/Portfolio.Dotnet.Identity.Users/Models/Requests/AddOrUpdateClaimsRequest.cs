namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class AddOrUpdateClaimsRequest : BaseUserNameRequest
    {
        public IEnumerable<ClaimDTO> Claims { get; set; } = [];
    }
}
