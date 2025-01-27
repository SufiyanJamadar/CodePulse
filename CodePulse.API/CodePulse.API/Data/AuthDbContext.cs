using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "d7fd772a-7c10-410f-85d5-f5f7a463fda8";
            var writerRoleId = "69b78625-29fe-40ac-a25f-289d38a9cdd3";

            // Create Reader and Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id= readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp=readerRoleId
                },
                new IdentityRole()
                {
                    Id= writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seed the roles

            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User
            var adminUserId = "e3d1a7ab-e217-45ca-9803-f62f9f5339f6";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            
            builder.Entity<IdentityUser>().HasData(admin);


            // Give Roles to Admin 

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId= adminUserId,
                    RoleId=readerRoleId
                },
                new()
                {
                    UserId= adminUserId,
                    RoleId=writerRoleId
                }

            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
