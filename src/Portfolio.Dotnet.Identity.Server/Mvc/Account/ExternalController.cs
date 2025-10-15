using Duende.IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Dotnet.Identity.Server.Mvc.Home.Models;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Models;
using System.Security.Claims;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IEventService events,
        ILogger<ExternalController> logger,
        IWebHostEnvironment environment,
        IUserService users) : Controller
    {
        private readonly IUserService _userService = users;
        private readonly IIdentityServerInteractionService _interaction = interaction;
        private readonly IClientStore _clientStore = clientStore;
        private readonly ILogger<ExternalController> _logger = logger;
        private readonly IEventService _events = events;
        private readonly IWebHostEnvironment _environment = environment;

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };

            return Challenge(props, scheme);

        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                var error = "External authentication error";
                return Error(error, result?.Failure?.Message);
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProvider(result);
            // this might be where you might initiate a custom workflow for user registration
            // in this sample we don't show how that would be done, as our sample implementation
            // simply auto-provisions new external user
            user ??= await LinkUser(provider, providerUserId, claims);
            //user ??= await AutoProvisionUser(provider, providerUserId, claims);

            if (user == null)
            {
                var error = "Cannot link users.";
                var description = $"Provider: {provider}, ProviderUserId: {providerUserId}";
                return Error(error, description);
            }

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.UserName,
                IdentityProvider = provider,
                AdditionalClaims = additionalLocalClaims
            };

            await HttpContext.SignInAsync(isuser, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties?.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId.ToString(), user.SubjectId, user.UserName, true, context?.Client.ClientId));

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnUrl);
                }
            }

            return Redirect(returnUrl);
        }

        private ViewResult Error(string error, string? errorDescription)
        {
            _logger.LogDebug("{error}: {errorDescription}", error, errorDescription);
            var vm = new ErrorViewModel
            {
                Error = new ErrorMessage
                {
                    Error = error
                }
            };
            if (_environment.IsDevelopment())
            {
                vm.Error.ErrorDescription = errorDescription;
            }
            return View("Error", vm);
        }

        private async Task<(UserDTO? user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProvider(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser?.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser?.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var loginProvider = result.Properties?.Items["scheme"];
            var providerKey = userIdClaim.Value;
            if (loginProvider == null)
            {
                throw new NullReferenceException(nameof(loginProvider));
            }
            if (providerKey == null)
            {
                throw new NullReferenceException(nameof(providerKey));
            }
            // find external user
            var user = await _userService.FindByExternalProvider(loginProvider, providerKey);

            return (user, loginProvider, providerKey, claims);
        }

        private async Task<UserDTO?> LinkUser(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            var user = await _userService.LinkUser(provider, providerUserId, [.. claims]);
            return user;
        }

        //private async Task<UserDTO> AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
        //{
        //    var user = await _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
        //    return user;
        //}

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private static void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties?.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens([new AuthenticationToken { Name = "id_token", Value = idToken }]);
            }
        }
    }
}
