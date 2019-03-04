using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        IEnumerable<UserDTO> GetUsers(string searchTerm, string sortBy);
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO> GetUserById(string userId);
        Task<IList<string>> GetUserRoles(string userId);
        Task<IEnumerable<RoleDTO>> GetAllRoles();
        Task<OperationDetails> CreateUser(UserDTO user, string[] selectedRoles);
        Task<OperationDetails> EditUser(UserDTO user, string[] selectedRoles);
        Task<OperationDetails> DeleteUser(string id);
        Task<ClaimsIdentity> Authenticate(UserLoginDTO loginUser);
        Task<OperationDetails> RegisterUser(UserDTO user);
    }
}
