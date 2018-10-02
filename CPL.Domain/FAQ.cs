using System;
using System.Collections.Generic;
using System.Text;
using CPL.Common.Models;

namespace CPL.Domain
{
    public class FAQ : Entity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int LangId { get; set; }
        public int GroupId { get; set; }

        public virtual Lang Lang { get; set; }
        public virtual Group Group { get; set; }
    }
}
