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
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<Order> Orders { get; set; }
        DbSet<Phone> Phones { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T t) where T : class;
    }
}
