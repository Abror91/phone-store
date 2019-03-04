using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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

namespace Phonix.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRoleService _roles;
        public AccountController(IRoleService roles)
        {
            _roles = roles;
        }
        private IUserService UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<IUserService>(); }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = new UserLoginDTO { Email = loginModel.Email, Password = loginModel.Password };
                var claim = await UserManager.Authenticate(user);
                if(claim == null)
                {
                    ModelState.AddModelError("", "Incorrect login or password.");
                    return View(loginModel);
                }
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return RedirectToAction("Index", "Home");
            }
            return View(loginModel);
        }
        
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    Address = model.Address,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await UserManager.RegisterUser(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}