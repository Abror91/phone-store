using Microsoft.AspNet.Identity.EntityFramework;
using Phonix.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Phone> Phones { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
        public new DbEntityEntry Entry<T>(T t) where T : class
        {
            return base.Entry(t);
        }
    }
}
