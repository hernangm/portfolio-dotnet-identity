using Microsoft.EntityFrameworkCore;
using Portfolio.Dotnet.Identity.Data.Users;

namespace Portfolio.Dotnet.Identity.Data.Factories
{
    public abstract class DesignTimeDbContextFactoryBase
    {
        public ThisIdentityDbContext CreateDbContext(string[] args)
        {
            var connectionString = string.Empty;
            var connectionArgIndex = Array.IndexOf(args, "--connection");
            if (connectionArgIndex != -1 && connectionArgIndex < args.Length - 1)
            {
                connectionString = args[connectionArgIndex + 1];
            }
            var optionsBuilder = new DbContextOptionsBuilder<ThisIdentityDbContext>();
            optionsBuilder.ConfigureDataContext(connectionString, false);
            return new ThisIdentityDbContext(optionsBuilder.Options);
        }
    }
}