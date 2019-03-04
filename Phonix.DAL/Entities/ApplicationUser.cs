using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
