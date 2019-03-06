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

        private async Task InditializeAdminAndRolesData(ApplicationDbContext db)
        {
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
                await roleManager.CreateAsync(new ApplicationRole { Name = "User" });

            }
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "abror@abror.com",
                    UserName = "abror@abror.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Abror?15");
                await userManager.AddToRolesAsync(admin.Id, new string[] { "Admin", "User" });
            }
        }
    }
}
