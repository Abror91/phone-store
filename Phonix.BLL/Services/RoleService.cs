using AutoMapper;
using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using Phonix.BLL.Interfaces;
using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _db;
        public RoleService(IUnitOfWork db)
        {
            _db = db;
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            var roles = _db.RoleManager.Roles;
            var mapper = new MapperConfiguration(c => c.CreateMap<ApplicationRole, RoleDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<RoleDTO>>(roles);
        }

        public async Task<RoleDTO> GetRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            var role = await _db.RoleManager.FindByIdAsync(id);
            if (role == null)
                throw new NullReferenceException();
            var r = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
            return r;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ArgumentNullException(nameof(roleId));
            var role = await _db.RoleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new NullReferenceException();
            var users = new List<UserDTO>();
            foreach(var user in _db.UserManager.Users.ToList())
            {
                if(await _db.UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    var u = new UserDTO
                    {
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email
                    };
                    users.Add(u);
                }
            }
            return users;
        }

        public async Task<OperationDetails> CreateRole(RoleDTO role)
        {
            if (role == null)
                return new OperationDetails(false, "Error. Role details cannot be empty!", "");
            if (string.IsNullOrEmpty(role.Name))
                return new OperationDetails(false, "Error. Role name cannot be empty!", "");
            var r = new ApplicationRole
            {
                Name = role.Name
            };
            var result = await _db.RoleManager.CreateAsync(r);
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            return new OperationDetails(true, "Success. New role was successfully created!", "");
        }

        public async Task<OperationDetails> EditRole(RoleDTO role)
        {
            if (role == null)
                return new OperationDetails(false, "Error. Role details cannot be empty!", "");
            if (string.IsNullOrEmpty(role.Name))
                return new OperationDetails(false, "Error. Role name cannot be empty!", "");
            var roleToEdit = await _db.RoleManager.FindByIdAsync(role.Id);
            if (roleToEdit == null)
                return new OperationDetails(false, "Error. Role was not found!", "");
            roleToEdit.Name = role.Name;
            var result = await _db.RoleManager.UpdateAsync(roleToEdit);
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            return new OperationDetails(true, "Success. Role was successfully updated", "");
        }

        public async Task<OperationDetails> DeleteRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return new OperationDetails(false, "Error. Role id cannot be empty!", "");
            var role = await _db.RoleManager.FindByIdAsync(roleId);
            if (role == null)
                return new OperationDetails(false, "Error. Role was not found!", "");
            var result = await _db.RoleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            return new OperationDetails(true, "Success. Role was successfully deleted!", "");
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
