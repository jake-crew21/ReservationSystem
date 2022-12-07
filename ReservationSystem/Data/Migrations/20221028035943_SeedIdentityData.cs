using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using ReservationSystem.Models;

#nullable disable

namespace ReservationSystem.Migrations
{
    public partial class SeedIdentityData : Migration
    {
        private MigrationBuilder? _migrationBuilder;
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder is available to other methods
            _migrationBuilder = migrationBuilder;

            //GUID generator https://guidgenerator.com/online-guid-generator.aspx
            //709f0502-61e5-4cb6-8006-62dea5efce60  --used
            //22fbfbba-8c7b-4151-a2c5-7bfaec9da649  --used
            //db743204-02d3-48df-8225-0200b67eb53b  --used
            //deccbc67-7dba-4807-b26b-dc4627bbf386  --used
            //44b008de-7d31-447c-863f-ef7dca23eaa8  --used

            //Add Roles
            string adminId = "709f0502-61e5-4cb6-8006-62dea5efce60";
            string userId = "db743204-02d3-48df-8225-0200b67eb53b";
            CreateRole(adminId, "Admin");
            CreateRole("22fbfbba-8c7b-4151-a2c5-7bfaec9da649", "Staff");
            CreateRole(userId, "User");
            //Add Users
            CreateUser("deccbc67-7dba-4807-b26b-dc4627bbf386", "Jarvis", "Smith", "seed1@test.com", "p@ssw0rd", "0400000000", new string[] {userId});
            CreateUser("44b008de-7d31-447c-863f-ef7dca23eaa8", "Dev", "Person", "dev@email.com", "Admin", "", new string[] { adminId });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _migrationBuilder = migrationBuilder;

            //Delete Roles
            DeleteData("709f0502-61e5-4cb6-8006-62dea5efce60");
            DeleteData("22fbfbba-8c7b-4151-a2c5-7bfaec9da649");
            DeleteData("db743204-02d3-48df-8225-0200b67eb53b");
            //Delete Users
            DeleteUser("deccbc67-7dba-4807-b26b-dc4627bbf386");
            DeleteUser("44b008de-7d31-447c-863f-ef7dca23eaa8");
        }

        /// <summary>
        /// Create an IdentityUser role
        /// </summary>
        private void CreateRole(string id, string name)
        {
            //TODO: Validation

            //role obj
            IdentityRole role = new IdentityRole();

            role.Id = id;
            role.Name = name;

            //Generate normalized name
            role.NormalizedName = name.ToUpperInvariant();

            //Concurrency stamp
            //A random value that must change whenever a user is persisted to the store
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            //Build query
            string[] columns = { "Id", "Name", "NormalizedName", "ConcurrencyStamp" };
            object[] values = { role.Id, role.Name, role.NormalizedName, role.ConcurrencyStamp };

            //insert record into the database
            _migrationBuilder.InsertData("AspNetRoles", columns, values);
        }

        /// <summary>
        /// Delete an IdentityUser Role
        /// </summary>
        /// <param name="id">NVARCHAR Id</param>
        private void DeleteData(string id)
        {
            //TODO: Validation

            //Remove role from db
            _migrationBuilder.DeleteData("AspNetRoles", "Id", id);
        }

        /// <summary>
        /// Create a IdentityUser
        /// </summary>
        private void CreateUser(string id, string firstName, string lastName, string email, string password, string? phone, string[]? roleIds = null)
        {
            //user obj
            ApplicationUser user = new ApplicationUser();
            //Assigned needed values
            user.Id = id;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserName = email;
            user.Email = email;
            user.PhoneNumber = phone;
            //Generate Normalization
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            //Generate Stamps
            //A random value that must change whenever a users credentials change (password changed, login removed)
            user.SecurityStamp = Guid.NewGuid().ToString();
            //A random value that must change whenever a user is persisted to the store
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            //Generate password hash from plaintext password
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            //other data
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;

            //build query
            string[] columns = { "Id", "FirstName", "LastName", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount" };
            object[] values = { user.Id, user.FirstName, user.LastName, user.UserName, user.NormalizedUserName, user.Email, user.NormalizedEmail, user.EmailConfirmed, user.PasswordHash, user.SecurityStamp, user.ConcurrencyStamp, user.PhoneNumber, user.PhoneNumberConfirmed, user.TwoFactorEnabled, user.LockoutEnd, user.LockoutEnabled, user.AccessFailedCount };

            //insert record into db
            _migrationBuilder.InsertData("AspNetUsers", columns, values);

            //Assign role(s) to user
            if (roleIds != null)
            {
                foreach (string roleId in roleIds)
                {
                    AssignRoleToUser(user.Id, roleId);
                }
            }
        }

        /// <summary>
        /// Delete an IdentityUser
        /// </summary>
        private void DeleteUser(string id)
        {
            _migrationBuilder.DeleteData("AspNetUsers", "Id", id);
        }

        /// <summary>
        /// Assign a Role to a User
        /// </summary>
        private void AssignRoleToUser(string userId, string roleId)
        {
            //TODO: Validation

            //Build query
            string[] columns = { "UserId", "RoleId" };
            object[] values = { userId, roleId };

            //insert record into the database
            _migrationBuilder.InsertData("AspNetUserRoles", columns, values);
        }
    }
}
