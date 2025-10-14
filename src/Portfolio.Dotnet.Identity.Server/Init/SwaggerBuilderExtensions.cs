namespace Portfolio.Dotnet.Identity.Server.Init
{
    internal static class SwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string appId, string apiVersion)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{appId} {apiVersion}");
                c.DocumentTitle = appId;
                c.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
