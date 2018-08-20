using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class LangContent : Entity
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string TableName { get; set; }
        public int TableId { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}
