using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Portfolio.Dotnet.Identity.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ThisIdentityDbContext>
    {
        public ThisIdentityDbContext CreateDbContext(string[] args)
        {
            var connectionString = string.Empty;
            //const string environmentVariable = "CONNSTR";
            //var connectionString = Environment.GetEnvironmentVariable(environmentVariable) ?? throw new Exception($"Cannot read connectionString from environment variable '{environmentVariable}'.");
            var connectionArgIndex = Array.IndexOf(args, "--connection");
            if (connectionArgIndex != -1 && connectionArgIndex < args.Length - 1)
            {
                connectionString = args[connectionArgIndex + 1];
            }
            var optionsBuilder = new DbContextOptionsBuilder<ThisIdentityDbContext>();
            optionsBuilder.ConfigureIdentityDataContext(connectionString, false);
            return new ThisIdentityDbContext(optionsBuilder.Options);
        }
    }
}
