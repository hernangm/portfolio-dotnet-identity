namespace Portfolio.Dotnet.Identity.Server.Mvc.Account.Models
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
