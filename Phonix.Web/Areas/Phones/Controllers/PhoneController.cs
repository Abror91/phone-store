using AutoMapper;
using Phonix.BLL.DTO;
using Phonix.BLL.Interfaces;
using Phonix.Web.Areas.Phones.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Areas.Phones.Controllers
{
    public class PhoneController : Controller
    {
        private readonly IUserService _users;
        private readonly IPhoneService _phones;
        public PhoneController(IPhoneService phones, IUserService users)
        {
            _phones = phones;
            _users = users;
        }
        // GET: Phones/Phone
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> PhonesList()
        {
            var phones = await _phones.GetPhones();
            var phonesToReturn = new List<PhoneViewModel>();
            foreach(var p in phones)
            {
                var phone = new PhoneViewModel
                {
                    Id = p.Id,
                    Model = p.Model,
                    CompanyName = p.CompanyName,
                    ReleaseDate = p.ReleaseDate.ToShortDateString()
                };
                phonesToReturn.Add(phone);
            }
            return Json(phonesToReturn, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetPhone(int id)
        {
            var phone = await _phones.GetPhoneById(id);
            var p = new PhoneViewModel
            {
                Id = phone.Id,
                Model = phone.Model,
                CompanyName = phone.CompanyName,
                ReleaseDate = phone.ReleaseDate.ToShortDateString()
            };
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        

        public async Task<JsonResult> AddPhone(PhoneViewModel phone)
        {
            if (ModelState.IsValid)
            {
                var p = new PhoneDTO
                {
                    Model = phone.Model,
                    CompanyName = phone.CompanyName,
                    ReleaseDate = Convert.ToDateTime(phone.ReleaseDate)
                };
                var result = await _phones.Add(p);
                if (result.Succeeded)
                {
                    return Json(result.Message, JsonRequestBehavior.AllowGet);
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> EditPhone(PhoneViewModel phone)
        {
            var p = new PhoneDTO
            {
                Id = phone.Id,
                Model = phone.Model,
                CompanyName = phone.CompanyName,
                ReleaseDate = Convert.ToDateTime(phone.ReleaseDate)
            };
            var result = await _phones.Update(p);
            return Json(result.Message, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeletePhone(int id)
        {
            var result = await _phones.Delete(id);
            return Json(result.Message, JsonRequestBehavior.AllowGet);
        }
    }
}