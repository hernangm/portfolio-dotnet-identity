namespace Portfolio.Dotnet.Identity.Users.Models.Responses
{
    public class UsersOperationResponse : BaseOperationResponse
    {
        public UsersOperationResponse(bool success, string error)
            : base(success, error)
        {
        }

        public UsersOperationResponse(string error)
            : base(string.IsNullOrEmpty(error), error)
        {
        }

        public UsersOperationResponse(bool success, IEnumerable<string>? errors)
            : base(success, errors)
        {
        }
    }
}
