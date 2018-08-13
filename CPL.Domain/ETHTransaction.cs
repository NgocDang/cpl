using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class ETHTransaction : Entity
    {
        public int Id { get; set; }
        public string TxHashId { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Status { get; set; }
    }
}
