using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ApplicationUserId { get; set; }
        public int PhoneId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Phone Phone { get; set; }
    }
}
