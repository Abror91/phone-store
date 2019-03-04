using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Interfaces
{
    public interface IRoleService : IDisposable
    {
        IEnumerable<RoleDTO> GetRoles();
        Task<RoleDTO> GetRole(string id);
        Task<IEnumerable<UserDTO>> GetUsersInRole(string roleId);
        Task<OperationDetails> CreateRole(RoleDTO role);
        Task<OperationDetails> EditRole(RoleDTO role);
        Task<OperationDetails> DeleteRole(string roleId);
    }
}
