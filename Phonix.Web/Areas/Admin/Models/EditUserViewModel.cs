using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}