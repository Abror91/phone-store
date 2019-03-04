using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Entities
{
    public  class Phone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string CompanyName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImagePath { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
