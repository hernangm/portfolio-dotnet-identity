using System.ComponentModel.DataAnnotations;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Account.Models
{
    public class LoginInputModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
