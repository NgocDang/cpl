using CPL.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ContactViewModel
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
    }
}
