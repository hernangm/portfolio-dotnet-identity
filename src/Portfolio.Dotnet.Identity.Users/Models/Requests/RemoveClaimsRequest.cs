namespace Portfolio.Dotnet.Identity.Users.Models.Requests
{
    public class RemoveClaimsRequest : BaseUserNameRequest
    {
        public IEnumerable<ClaimDTO> Claims { get; set; } = [];
    }
}
