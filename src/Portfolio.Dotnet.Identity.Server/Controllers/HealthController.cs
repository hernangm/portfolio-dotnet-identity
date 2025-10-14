using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Dotnet.Identity.Server.Controllers
{
    [ApiVersionNeutral]
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public IActionResult Health()
        {
            return Ok(new { date = DateTime.UtcNow });
        }
    }
}

