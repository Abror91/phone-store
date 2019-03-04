using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Interfaces
{
    public interface IPhoneService : IDisposable
    {
        Task<IEnumerable<PhoneDTO>> GetPhones();
        Task<PhoneDTO> GetPhoneById(int? id);
        Task<OperationDetails> Add(PhoneDTO phone);
        Task<OperationDetails> Update(PhoneDTO phone);
        Task<OperationDetails> Delete(int? id);
    }
}
