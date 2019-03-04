namespace Phonix.DAL.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Phonix.DAL.Entities;
    using Phonix.DAL.Repositories;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<Phonix.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Phonix.DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //AddData(context);
            InditializeAdminData();
        }

        private void AddData(ApplicationDbContext db)
        {
            db.Phones.AddOrUpdate(new Phone
            {
                Model = "S5",
                CompanyName = "Samsung",
                ReleaseDate = new DateTime(2015, 10, 10)
            });
            db.Phones.AddOrUpdate(new Phone
            {
                Model = "S6",
                CompanyName = "Apple",
                ReleaseDate = new DateTime(2015, 10, 10)
            });
            db.Phones.AddOrUpdate(new Phone
            {
                Model = "S7",
                CompanyName = "Asus",
                ReleaseDate = new DateTime(2015, 10, 10)
            });
            db.Phones.AddOrUpdate(new Phone
            {
                Model = "S8",
                CompanyName = "Meisus",
                ReleaseDate = new DateTime(2015, 10, 10)
            });
            db.Phones.AddOrUpdate(new Phone
            {
                Model = "S9",
                CompanyName = "Xiamo",
                ReleaseDate = new DateTime(2015, 10, 10)
            });
            db.SaveChanges();

        }

        private async Task InditializeAdminData()
        {
            var _db = new UnitOfWork();
            var userManager = _db.UserManager;
            var roleManager = _db.RoleManager;
            const string name = "abror@abror.com";
            const string password = "Abror?15";
            const string adminRole = "Admin";
            const string userRole = "User";

            var role = await roleManager.FindByNameAsync(adminRole);
            if(role == null)
            {
                role = new ApplicationRole { Name = adminRole };
                var result = await roleManager.CreateAsync(role);
            }
            var role2 = await roleManager.FindByNameAsync(userRole);
            if(role2 == null)
            {
                role2 = new ApplicationRole { Name = userRole };
                var result = await roleManager.CreateAsync(role2);
            }
            var user = await userManager.FindByNameAsync(name);
            if(user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = await userManager.CreateAsync(user, password);
                result = await userManager.SetLockoutEnabledAsync(user.Id, false);
            }
            // Add user admin to Role Admin if not already added
            var rolesForUser = await userManager.GetRolesAsync(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRolesAsync(user.Id, role.Name);
            }
            if (!rolesForUser.Contains(role2.Name))
            {
                var result = userManager.AddToRolesAsync(user.Id, role2.Name);
            }
        }
    }
}
