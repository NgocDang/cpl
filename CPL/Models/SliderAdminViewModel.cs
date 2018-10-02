using CPL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class SliderAdminViewModel
    {
        public SliderAdminViewModel()
        {
            SliderDetails = new List<SliderDetailAdminViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int GroupId { get; set; }
        public int Status { get; set; }

        public List<GroupViewModel> Groups { get; set; }
        public List<SliderDetailAdminViewModel> SliderDetails { get; set; }
    }
}
