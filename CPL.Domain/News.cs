using CPL.Common.Models;
using System;

namespace CPL.Domain
{
    public class News : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
