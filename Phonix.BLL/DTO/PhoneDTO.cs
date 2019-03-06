﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.DTO
{
    public class PhoneDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string CompanyName { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
