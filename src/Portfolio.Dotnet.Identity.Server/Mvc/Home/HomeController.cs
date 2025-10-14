using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Dotnet.Identity.Server.Mvc.Home.Models;

namespace Portfolio.Dotnet.Identity.Server.Mvc.Home
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Route("home")]
    public class HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger) : Controller
    {
        private readonly IIdentityServerInteractionService _interaction = interaction;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly ILogger _logger = logger;

        [Route("")]
        public IActionResult Index()
        {
            if (_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            _logger.LogInformation("Homepage is disabled in production. Returning 404.");
            return NotFound();
        }

        [Route("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
    }
}