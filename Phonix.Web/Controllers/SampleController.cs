using Phonix.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Controllers
{
    public class SampleController : Controller
    {
        List<Sample> samples = new List<Sample>();
        private IEnumerable<Sample> AddData()
        {
            for (int i = 0; i < 10; i++)
            {
                var sample = new Sample
                {
                    Id = 1 + i,
                    Name = "Johnson"+ i.ToString(),
                    City = "New York" + i.ToString(),
                    Country = "USA" + i.ToString()
                };
                samples.Add(sample);
            }
            return samples;
        }
        // GET: Sample
        public ActionResult Index()
        {
            var samples = AddData();
            return View(samples);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sample sample)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Request.Files[0].FileName);
                    var filePathOfWebsite = "~/Images/Phones" + fileName;
                    Request.Files[0].SaveAs(Server.MapPath(filePathOfWebsite));
                    sample.ImagePath = filePathOfWebsite;
                }
                samples.Add(sample);
                return RedirectToAction("Index");
            }
            return View(sample);
        }
    }
}