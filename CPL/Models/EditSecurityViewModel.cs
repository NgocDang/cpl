using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Models
{
    public class KYCViewModel
    {
        public int Id { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public bool? KYCVerified { get; set; }
        public DateTime? KYCCreatedDate { get; set; }
        public IFormFile FrontSideImage { set; get; }
        public IFormFile BackSideImage { set; get; }
    }
}
