using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Phonix.Web.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}