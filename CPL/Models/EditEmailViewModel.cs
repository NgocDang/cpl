using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class UpdateEmailViewModel
    {
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
        public string NewEmailConfirm { get; set; }
    }
}
