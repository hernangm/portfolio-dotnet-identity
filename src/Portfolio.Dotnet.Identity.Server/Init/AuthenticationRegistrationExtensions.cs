using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Dotnet.Identity.Server.Config;

namespace Portfolio.Dotnet.Identity.Server.Init
{
    public static class AuthenticationRegistrationExtensions
    {
        public static void RegisterAuthentication(this IServiceCollection services, ExternalIdentityProvidersSettings externalIdentityProvidersSettings)
        {
            services.AddLocalApiAuthentication();

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });

            authenticationBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            if (externalIdentityProvidersSettings.Azure.Enabled)
            {
                authenticationBuilder.AddOpenIdConnect("aad", "Login with Microsoft", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = $"https://login.windows.net/{externalIdentityProvidersSettings.Azure.TenantId}";
                    options.ClientId = externalIdentityProvidersSettings.Azure.ClientId;
                    options.ClientSecret = externalIdentityProvidersSettings.Azure.ClientSecret;
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.SaveTokens = true;
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "email"
                    };
                    options.CallbackPath = "/signin-aad";
                    options.Prompt = "select_account";
                });
            }
            if (externalIdentityProvidersSettings.Google.Enabled)
            {
                authenticationBuilder.AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = externalIdentityProvidersSettings.Google.ClientId;
                    options.ClientSecret = externalIdentityProvidersSettings.Google.ClientSecret;
                    options.CallbackPath = "/signin-google";
                });
            }
        }
    }
}
