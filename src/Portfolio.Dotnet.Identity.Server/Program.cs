using IdentityServer4.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Portfolio.Dotnet.Identity.Email.Registration;
using Portfolio.Dotnet.Identity.Server.Config;
using Portfolio.Dotnet.Identity.Server.Infra;
using Portfolio.Dotnet.Identity.Server.Init;
using Portfolio.Dotnet.Identity.Users.Data.Infra;

internal class Program
{
    private static void Main(string[] args)
    {
        const string AllowAllOrigins = "AllowAllOrigins";
        const string Version = "v1";
        var AppId = typeof(Program).Namespace!;

        var builder = WebApplication.CreateBuilder(args);
        var applicationSettings = builder.Services.RegisterApplicationSettings<ApplicationSettings>(builder.Configuration);

        builder.Services.AddRazorPages();
        builder.Services.AddHealthChecks().RegisterHealthChecks(applicationSettings);

        builder.Services.AddDataProtection()
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(Version, new OpenApiInfo { Title = AppId, Version = Version });
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AllowAllOrigins, builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        });
        builder.Services.AddControllers(o =>
        {
            o.Conventions.Add(new ActionHidingConvention());
        });

        builder.Services.AddSingleton<ICorsPolicyService>((container) =>
        {
            var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
            return new DefaultCorsPolicyService(logger)
            {
                AllowAll = true
            };
        });

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("connectionString");
        void configureAction(IServiceProvider provider, DbContextOptionsBuilder builder)
        {
            builder.ConfigureDataContext(connectionString, applicationSettings.IsProduction());
        }

        //builder.Services.RegisterApplications(applicationSettings);
        builder.Services.RegisterAuthentication(applicationSettings.ExternalIdentityProviders);
        builder.Services.RegisterIdentity(applicationSettings, configureAction);
        builder.Services.RegisterIdentityServer(applicationSettings.IssuerUrl ?? string.Empty, configureAction);
        builder.Services.RegisterEmail(applicationSettings.Email, new Uri(AppDomain.CurrentDomain.BaseDirectory).AbsolutePath, applicationSettings.Environment, applicationSettings.IsProduction());


        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            IdentityModelEventSource.ShowPII = true;
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapHealthChecks("/health"); // Map the health check endpoint
        app.UseCors(AllowAllOrigins);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllers();
        app.MapRazorPages().RequireAuthorization();
        app.UseSwagger(AppId, Version);
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always,
            MinimumSameSitePolicy = SameSiteMode.Unspecified
        });


        //app.Use(async (ctx, next) =>
        //{
        //    if (!string.IsNullOrEmpty(resolvedAppSettings.IssuerUrl))
        //    {
        //        ctx.SetIdentityServerOrigin(resolvedAppSettings.IssuerUrl);
        //    }
        //    await next();
        //});




        app.Run();

    }
}
