using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Portfolio.Dotnet.Identity.Data
{
    public static class DatabaseConfigurationUtilities
    {
        public static DbContextOptionsBuilder ConfigureDataContext(this DbContextOptionsBuilder builder, string? connectionString, bool isProduction)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                builder.UseNpgsql();
            }
            else
            {
                builder.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    sqlOptions.EnableRetryOnFailure();
                });
            }
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
