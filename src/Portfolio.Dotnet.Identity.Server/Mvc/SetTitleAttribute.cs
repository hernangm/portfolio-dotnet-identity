using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portfolio.Dotnet.Identity.Server.Config;

namespace Portfolio.Dotnet.Identity.Server.Mvc
{
    public class SetTitleAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is Controller controller)
            {
                var applicationSettings = filterContext.HttpContext.RequestServices.GetService<ApplicationSettings>();
                controller.ViewBag.Title = applicationSettings?.UI?.Title ?? "<Title>";
            }
        }
    }
}
