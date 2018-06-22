using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LangDetail : Entity
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual Lang Lang { get; set; }
    }
}
