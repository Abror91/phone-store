using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phonix.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public int PhoneId { get; set; }
        public string PhoneName { get; set; }
    }
}