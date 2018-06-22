using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class Currency : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
