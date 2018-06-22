using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Notification : Entity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LangId { get; set; }
        public virtual Lang Lang { get; set; }
    }
}
