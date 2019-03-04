using Phonix.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Interfaces
{
    public interface IPhoneRepository : IDisposable
    {
        Task<IEnumerable<Phone>> GetPhones();
        Task<Phone> GetPhone(int? id);
        Task Add(Phone phone);
        Task Update(Phone phone);
        Task Delete(Phone phone);
    }
}
