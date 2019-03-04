using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public int PhoneId { get; set; }
        public string PhoneName { get; set; }
    }
}
