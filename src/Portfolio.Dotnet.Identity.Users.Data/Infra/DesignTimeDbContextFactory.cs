using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Portfolio.Dotnet.Identity.Users.Data.Infra
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ThisIdentityDbContext>
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
