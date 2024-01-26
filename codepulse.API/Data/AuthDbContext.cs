using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Data
{
    public class AuthDbContext : IdentityDbContext 

    {
        public AuthDbContext(DbContextOptions options) : base(options) 
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "1ef4746e-cb0b-4b02-9287-55c24b0f5d1d";
            var writerRoleId = "66e6734f-bf00-490e-b34b-00ef7fbd021b";

            //create reader and writer role 
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },


                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId

                    
                }
            };


            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //create an Admin user 
            var adminUserId = "8857a5ba-38c8-4bcb-9aae-57ec5f3cd198";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin243@yahoo.com",
                NormalizedEmail = "admin243@yahoo.com".ToUpper(),
                NormalizedUserName = "admin243@yahoo.com".ToUpper()

            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);

            //Give role to Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId =adminUserId,
                    RoleId = writerRoleId
                }

            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);



             
        }

    }
}
