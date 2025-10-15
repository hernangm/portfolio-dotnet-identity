using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Portfolio.Dotnet.Identity.Data
{
    public static class DatabaseConfigurationUtilities
    {
        //public static Action<IServiceProvider, DbContextOptionsBuilder> ConfigureDbContextOptionsBuilder(string connectionString, bool isProduction)
        //{
        //    void b(IServiceProvider provider, DbContextOptionsBuilder builder)
        //    {
        //        builder.ConfigureIdentityDataContext(connectionString, isProduction);
        //    }
        //    return b;
        //}

        public static DbContextOptionsBuilder ConfigureIdentityDataContext(this DbContextOptionsBuilder builder, string connectionString, bool isProduction)
        {
            builder.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                //sqlOptions.MigrationsAssembly(assemblyName);
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
            return builder;
        }
    }
}
