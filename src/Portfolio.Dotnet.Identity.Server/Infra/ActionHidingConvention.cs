using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Portfolio.Dotnet.Identity.Server.Infra
{
    public class ActionHidingConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (typeof(Controller).IsAssignableFrom(action.Controller.ControllerType))
            {
                action.ApiExplorer.IsVisible = false;
            }
        }
    }
}
