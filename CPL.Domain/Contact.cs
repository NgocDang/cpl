using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Contact : Entity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
