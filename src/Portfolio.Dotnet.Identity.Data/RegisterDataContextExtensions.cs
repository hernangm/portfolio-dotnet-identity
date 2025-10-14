using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Portfolio.Dotnet.Identity.Data
{
    public static class RegisterDataContextExtensions
    {
        public static Action<IServiceProvider, DbContextOptionsBuilder> ConfigureSqlServer(string? assemblyName, string connectionString, bool isProduction)
        {
            void b(IServiceProvider provider, DbContextOptionsBuilder builder)
            {
                builder.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    sqlOptions.MigrationsAssembly(assemblyName);
                    sqlOptions.EnableRetryOnFailure();
                });

                builder.ConfigureWarnings(w =>
                {
                    if (isProduction)
                    {
                        w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning);
                    }
                    else
                    {
                        w.Throw(RelationalEventId.MultipleCollectionIncludeWarning);
                    }
                });
            }
            return b;
        }
    }
}
