using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly IApplicationDbContext _db;
        private readonly IDbSet<Phone> _phones;
        public PhoneRepository(IApplicationDbContext db)
        {
            _db = db;
            _phones = db.Set<Phone>();
        }

        public async Task<IEnumerable<Phone>> GetPhones()
        {
            return await _phones.ToListAsync();
        }
        public async Task<Phone> GetPhone(int? id)
        {
            return await _phones.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(Phone phone)
        {
            _phones.Add(phone);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Phone phone)
        {
            ChangeState(phone, EntityState.Modified);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Phone phone)
        {
            if (!_phones.Local.Contains(phone))
                _phones.Attach(phone);
            _phones.Remove(phone);
            ChangeState(phone, EntityState.Deleted);
            await _db.SaveChangesAsync();
        }
        

        public void Dispose()
        {
            _db.Dispose();
        }


        private void ChangeState(Phone phone, EntityState state)
        {
            _db.Entry(phone).State = state;
        }
    }
}
