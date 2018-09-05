using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class AgencyToken : Entity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? SysUserId { get; set; }
    }
}
