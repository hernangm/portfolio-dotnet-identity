using Portfolio.Dotnet.Identity.Server.Mvc.Consent.Models;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Device.Models
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; } = string.Empty;
    }
}