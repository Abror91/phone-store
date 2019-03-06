using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phonix.Web.Areas.Phones.Models
{
    public class PhoneViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string CompanyName { get; set; }
        public string ReleaseDate { get; set; }
    }
}