using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc.Enums
{
    public enum EnumPeriodInDay
    {
        [Display(Name = "24H")]
        _24H = 1,
        [Display(Name = "7D")]
        _7D = 7,
        [Display(Name = "31D")]
        _31D = 31,
        [Display(Name = "90D")]
        _90D = 90,
        [Display(Name = "30W")]
        _30W = 210,
        [Display(Name = "12M")]
        _12M = 365,
        [Display(Name = "5Y")]
        _5Y = 1825,
    }
}
