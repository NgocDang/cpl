using CPL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int GroupId { get; set; }
        public int Status { get; set; }

        public List<GroupViewModel> Groups { get; set; }
        public List<SliderDetailViewModel> SliderDetails { get; set; }
    }
}
