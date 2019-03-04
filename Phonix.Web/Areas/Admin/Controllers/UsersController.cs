using AutoMapper;
using PagedList;
using PagedList.Mvc;
using Phonix.BLL.DTO;
using Phonix.BLL.Interfaces;
using Phonix.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _users;
        public UsersController(IUserService users)
        {
            _users = users;
        }
        // GET: Admin/Users
        public ActionResult Index(string searchTerm, string sortBy, int? page)
        {
            ViewBag.EmailSortParam = string.IsNullOrWhiteSpace(sortBy) ? "email_desc" : "";
            ViewBag.AddressSortParam = sortBy == "address" ? "address_desc" : "address";

            var users =  _users.GetUsers(searchTerm, sortBy).AsQueryable();
            var mapper = new MapperConfiguration(c => c.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            var usersToReturn = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(users).ToPagedList(page ?? 1, 2);
            return View(usersToReturn);
        }

        public async Task<ActionResult> Details(string email)
        {
            var user = await _users.GetUserByEmail(email);
            var mapper = new MapperConfiguration(c => c.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            var userToReturn = mapper.Map<UserDTO, UserViewModel>(user);
            ViewBag.Roles = await _users.GetUserRoles(userToReturn.Id);
            return View(userToReturn);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _users.GetAllRoles(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };
                var result = await _users.CreateUser(user, selectedRoles);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            ViewBag.Roles = new SelectList(await _users.GetAllRoles(), "Name", "Name");
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await GetUserForEdit(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    Id = model.Id,
                    Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _users.EditUser(user, selectedRoles);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            var userForView = await GetUserForEdit(model.Id);
            return View(userForView);
        }

        private async Task<EditUserViewModel> GetUserForEdit(string userId)
        {
            var user = await _users.GetUserById(userId);
            var userRoles = await _users.GetUserRoles(user.Id);
            var allRoles = await _users.GetAllRoles();
            var userToReturn = new EditUserViewModel
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RolesList = allRoles.ToList().Select(s => new SelectListItem
                {
                    Selected = userRoles.Contains(s.Name),
                    Value = s.Name,
                    Text = s.Name
                })

            };
            return userToReturn;
        }

        public async Task<ActionResult> Delete(string id, string message)
        {
            var user = await _users.GetUserById(id);
            var userToDelete = new UserViewModel
            {
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                Password = user.PhoneNumber
            };
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorMessage = message;
            }
            return View(userToDelete);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(UserViewModel user)
        {
            var result = await _users.DeleteUser(user.Id);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return RedirectToAction("Delete", new { message = result.Message });
        }
    }
}