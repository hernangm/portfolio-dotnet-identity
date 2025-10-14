using Portfolio.Dotnet.Identity.Server.Mvc.Consent.Models;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Device.Models
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; } = string.Empty;
        public bool ConfirmUserCode { get; set; }
    }
}