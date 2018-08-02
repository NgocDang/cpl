using CPL.Misc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class ContactIndexViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int Category { get; set; }

        public SysUserViewModel SysUser { get; set; }
    }
}
