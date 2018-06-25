using CPL.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Domain
{
    public class GameHistory : Entity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int SysUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal Money { get; set; }
        public bool? Result { get; set; }
        public decimal Bonus { get; set; }

        public virtual Game Game { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
