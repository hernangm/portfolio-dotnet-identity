namespace Portfolio.Dotnet.Identity.Users.Models.Responses
{
    public class UserCreationOperationResponse : BaseOperationResponse
    {
        public int UserId { get; set; }

        public UserCreationOperationResponse(bool success, string error)
            : base(success, error)
        {
        }

        public UserCreationOperationResponse(string error)
            : base(string.IsNullOrEmpty(error), error)
        {
        }

        public UserCreationOperationResponse(bool success, IEnumerable<string>? errors)
            : base(success, errors)
        {
        }
    }
}
