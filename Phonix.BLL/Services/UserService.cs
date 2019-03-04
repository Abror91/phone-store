using AutoMapper;
using Microsoft.AspNet.Identity;
using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using Phonix.BLL.Interfaces;
using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phonix.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        public UserService(IUnitOfWork db)
        {
            _db = db;
        }

        public IEnumerable<UserDTO> GetUsers(string searchTerm, string sortBy)
        {
            var users = GetFilteredUsers(searchTerm);
            users = GetSortedUsers(users, sortBy);
            var mapper = new MapperConfiguration(c => c.CreateMap<ApplicationUser, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDTO>>(users.ToList());
        }

        private IQueryable<ApplicationUser> GetSortedUsers(IQueryable<ApplicationUser> users, string sortBy)
        {
            switch (sortBy)
            {
                case "address":
                    users.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    users.OrderByDescending(s => s.Address);
                    break;
                case "email_desc":
                    users.OrderByDescending(s => s.Email);
                    break;
                default:
                    users.OrderBy(s => s.Email);
                    break;
            }
            return users;
        }

        private IQueryable<ApplicationUser> GetFilteredUsers(string searchTerm)
        {
            IQueryable<ApplicationUser> users = _db.UserManager.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                users = users.Where(s => s.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            return users;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            var user = await _db.UserManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException();
            var u = new UserDTO
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return u;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            var user = await _db.UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new NullReferenceException();
            var u = new UserDTO
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return u;
        }

        public async Task<IList<string>> GetUserRoles(string userId)
        {
            return await _db.UserManager.GetRolesAsync(userId);

        }

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            var roles = await _db.RoleManager.Roles.ToListAsync();
            var rolesToReturn = new List<RoleDTO>();
            foreach(var r in roles)
            {
                var role = new RoleDTO { Id = r.Id, Name = r.Name };
                rolesToReturn.Add(role);
            }
            return rolesToReturn;
        }

        public async Task<OperationDetails> CreateUser(UserDTO user, string[] selectedRoles)
        {
            if (user == null)
                return new OperationDetails(false, "Error. User details are empty!", "");
            if (string.IsNullOrEmpty(user.Address))
                return new OperationDetails(false, "Error. User address cannot be empty!", "");
            if (string.IsNullOrEmpty(user.Email))
                return new OperationDetails(false, "Error. User Email cannot be empty!", "");
            if (string.IsNullOrEmpty(user.PhoneNumber))
                return new OperationDetails(false, "Error. User Phone number cannot be empty!", "");
            if (string.IsNullOrEmpty(user.Password))
                return new OperationDetails(false, "Error. User password cannot be empty!", "");
            var u = await _db.UserManager.FindByEmailAsync(user.Email);
            if(u == null)
            {
                var userToCreate = new ApplicationUser
                {
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                var result = await _db.UserManager.CreateAsync(userToCreate, user.Password);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
                if(selectedRoles != null)
                {
                    var assignToRoleResult = await _db.UserManager.AddToRolesAsync(userToCreate.Id, selectedRoles);
                    if (!assignToRoleResult.Succeeded)
                    {
                        return new OperationDetails(false, assignToRoleResult.Errors.FirstOrDefault(), "");
                    }
                }
                return new OperationDetails(true, "Success. New user created successfully!", "");
            }
            return new OperationDetails(false, "Error. The user with the given login already exists.", "");
        }

        public async Task<OperationDetails> EditUser(UserDTO user, string[] selectedRoles)
        {
            if (user == null)
                return new OperationDetails(false, "Error. User details are empty!", "");
            if (string.IsNullOrEmpty(user.Address))
                return new OperationDetails(false, "Error. User address cannot be empty!", "");
            if (string.IsNullOrEmpty(user.Email))
                return new OperationDetails(false, "Error. User Email cannot be empty!", "");
            if (string.IsNullOrEmpty(user.PhoneNumber))
                return new OperationDetails(false, "Error. User Phone number cannot be empty!", "");
            var userToEdit = await _db.UserManager.FindByIdAsync(user.Id);
            if (userToEdit == null)
                return new OperationDetails(false, "Error. User was not found!", "");
            userToEdit.Address = user.Address;
            userToEdit.Email = user.Email;
            userToEdit.PhoneNumber = user.PhoneNumber;
            var userRoles = await _db.UserManager.GetRolesAsync(userToEdit.Id);
            selectedRoles = selectedRoles ?? new string[] { };
            var result = await _db.UserManager.AddToRolesAsync(userToEdit.Id, selectedRoles.Except(userRoles).ToArray());
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            result = await _db.UserManager.RemoveFromRolesAsync(userToEdit.Id, userRoles.Except(selectedRoles).ToArray());
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            return new OperationDetails(true, "Success. User details are successfully updated!", "");
        }

        public async Task<OperationDetails> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new OperationDetails(false, "Error. User id was not recieved!", "");
            var user = await _db.UserManager.FindByIdAsync(id);
            if (user == null)
                return new OperationDetails(false, "Error. User was not found!", "");
            await _db.Orders.DeleteUserOrders(user.Id);
            var result = await _db.UserManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            return new OperationDetails(true, "Success. User was successfully deleted!", "");
        }

        public async Task<ClaimsIdentity> Authenticate(UserLoginDTO loginUser)
        {
            if (loginUser == null)
                throw new ArgumentNullException(nameof(loginUser));
            ClaimsIdentity claim = null;
            var user = await _db.UserManager.FindAsync(loginUser.Email, loginUser.Password);
            if(user != null)
            {
                claim = await _db.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public async Task<OperationDetails> RegisterUser(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var userObj = await _db.UserManager.FindByEmailAsync(user.Email);
            if (userObj == null)
            {
                var userToCreate = new ApplicationUser
                {
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                var result = await _db.UserManager.CreateAsync(userToCreate, user.Password);
                if(result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
                const string roleName = "User";
                result = await _db.UserManager.AddToRoleAsync(userToCreate.Id, roleName);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
                return new OperationDetails(true, "User successfully created!", "");
            }
            return new OperationDetails(false, "The user with the given login already exists!", "");
        }
        
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
