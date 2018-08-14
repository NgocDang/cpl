using CPL.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.Common.Enums
{
    public enum EnumCoinTransactionStatus
    {
        [CSS("badge-info")]
        PENDING,
        [CSS("badge-success")]
        SUCCESS,
        [CSS("badge-danger")]
        FAIL
    }
}
