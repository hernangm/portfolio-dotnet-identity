using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Dotnet.Identity.Data
{
    public class ThisIdentityDbContext(DbContextOptions<ThisIdentityDbContext> options) : IdentityDbContext<ThisUser, ThisRole, int, ThisUserClaim, ThisUserRole, ThisUserLogin, ThisRoleClaim, ThisUserToken>(options)
    {
        private const string SCHEMA = "identity";

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var userConfiguration = builder.Entity<ThisUser>();
            userConfiguration.ToTable("Users", SCHEMA, tb => tb.UseSqlOutputClause(false));
            userConfiguration.Property(m => m.PhoneNumber).IsRequired(false);

            builder.Entity<ThisRole>().ToTable("Roles", SCHEMA);
            builder.Entity<ThisUserClaim>().ToTable("UserClaims", SCHEMA, tb => tb.UseSqlOutputClause(false))
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserClaims)
                .HasForeignKey(uc => uc.UserId);
            builder.Entity<ThisUserRole>().ToTable("UserRoles", SCHEMA)
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            builder.Entity<ThisUserLogin>().ToTable("UserLogins", SCHEMA)
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLogins)
                .HasForeignKey(ul => ul.UserId);
            builder.Entity<ThisRoleClaim>().ToTable("RoleClaims", SCHEMA)
                .HasOne(rc => rc.Role)
                .WithMany(r => r.RoleClaims)
                .HasForeignKey(rc => rc.RoleId);
            builder.Entity<ThisUserToken>().ToTable("UserTokens", SCHEMA);
        }
    }
}