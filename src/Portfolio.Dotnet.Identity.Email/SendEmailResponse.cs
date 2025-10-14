namespace Portfolio.Dotnet.Identity.Email
{
    public class SendEmailResponse
    {
        public string MessageId { get; set; } = string.Empty;

        public IList<string> ErrorMessages { get; set; }

        public bool Successful => !ErrorMessages.Any();

        public SendEmailResponse()
        {
            ErrorMessages = [];
        }
    }
}
