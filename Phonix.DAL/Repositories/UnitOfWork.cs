using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IOrderRepository _orders;
        private readonly IPhoneRepository _phones;
        public UnitOfWork()
        {
            _db = new ApplicationDbContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _orders = new OrderRepository(_db);
            _phones = new PhoneRepository(_db);
        }
        public ApplicationUserManager UserManager { get
            {
                _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                _userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = false,
                };
                return _userManager;
            } }
        public ApplicationRoleManager RoleManager { get { return _roleManager; } }
        public IOrderRepository Orders { get { return _orders; } }
        public IPhoneRepository Phones { get { return _phones; } }
        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _roleManager.Dispose();
            _orders.Dispose();
            _phones.Dispose();
        }
    }
}
