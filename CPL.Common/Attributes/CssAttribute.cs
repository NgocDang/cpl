using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.Common.Attributes
{
    public class CSSAttribute : Attribute
    {
        public string Name { get; set; }

        public CSSAttribute(string name)
        {
            this.Name = name;
        }
    }
}
