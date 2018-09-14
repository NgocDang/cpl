using System.Collections.Generic;

namespace CPL.Models
{
    public class LangViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IList<LangDetailViewModel> LangDetails { get; set; }
    }
}
