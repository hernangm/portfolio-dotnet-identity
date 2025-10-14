namespace Portfolio.Dotnet.Identity.Users.Models.Responses
{
    public abstract class BaseOperationResponse
    {
        protected BaseOperationResponse(bool success, string error)
        {
            Success = success;
            Errors = [];
            if (!string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
            }
        }

        public BaseOperationResponse(string error)
            : this(!string.IsNullOrEmpty(error), error)
        {
        }

        public BaseOperationResponse(bool success, IEnumerable<string>? errors)
        {
            Success = success;
            Errors = [];
            if (errors != null)
            {
                Errors.AddRange(errors);
            }
        }

        public bool Success { get; private set; }
        public List<string> Errors { get; private set; }
    }
}
