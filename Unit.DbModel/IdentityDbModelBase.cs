using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Unit.DbModel
{
    /// <summary>
    /// 拥有一个主键的数据表骨架
    /// </summary>
    public abstract class IdentityDbModelBase:DbModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
    }
}
