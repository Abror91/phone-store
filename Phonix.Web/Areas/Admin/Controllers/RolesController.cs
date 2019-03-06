using AutoMapper;
using Phonix.BLL.DTO;
using Phonix.BLL.Interfaces;
using Phonix.Web.Areas.Admin.Models;
using Phonix.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = RolesList.Admin)]
    public class RolesController : Controller
    {
        private readonly IRoleService _roles;
        public RolesController(IRoleService roles)
        {
            _roles = roles;
        }

        public ActionResult Index()
        {
            var roles = _roles.GetRoles();
            var mapper = new MapperConfiguration(c => c.CreateMap<RoleDTO, RoleViewModel>()).CreateMapper();
            var rolesToReturn = mapper.Map<IEnumerable<RoleDTO>, IEnumerable<RoleViewModel>>(roles);
            return View(rolesToReturn);
        }

        public async Task<ActionResult> Details(string id)
        {
            var role = await _roles.GetRole(id);
            var r = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            var users = await _roles.GetUsersInRole(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            ViewBag.Users = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(users);
            return View(r);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var r = new RoleDTO
                {
                    Name = role.Name
                };
                var result = await _roles.CreateRole(r);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                ModelState.AddModelError(result.Property, result.Message);
            }
            return View(role);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roles.GetRole(id);
            var r = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var r = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                };
                var result = await _roles.EditRole(r);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                ModelState.AddModelError(result.Property, result.Message);
            }
            return View(role);
        }

        public async Task<ActionResult> Delete(string id, string message)
        {
            var role = await _roles.GetRole(id);
            var r = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            if(!string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorMessage = message;
            }
            return View(r);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(RoleViewModel role)
        {
            var result = await _roles.DeleteRole(role.Id);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return RedirectToAction("Delete", new { message = result.Message });
        }
    }
}