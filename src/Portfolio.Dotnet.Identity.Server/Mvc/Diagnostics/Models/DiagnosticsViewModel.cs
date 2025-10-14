using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Text;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Diagnostics.Models
{
    public class DiagnosticsViewModel
    {
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            AuthenticateResult = result;

            if (result.Properties?.Items.ContainsKey("client_list") ?? false)
            {
                var encoded = result.Properties.Items["client_list"]!;
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);

                Clients = JsonConvert.DeserializeObject<string[]>(value) ?? throw new Exception("Cannot deserialize value");
            }
        }

        public AuthenticateResult AuthenticateResult { get; }
        public IEnumerable<string> Clients { get; } = [];
    }
}