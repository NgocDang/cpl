using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class ETHTransaction : Entity
    {
        public int Id { get; set; }
        public string TxId { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
