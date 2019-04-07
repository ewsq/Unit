using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Unit.DbModel
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SUser : IdentityUser<ulong>
    {
        public SUser()
        {
        }



        /// <summary>
        /// 头像地址
        /// </summary>
        [MaxLength(64)]
        public string HeadImg { get; set; }

        public ICollection<ComplateUnit> ComplateUnits { get; set; }

    }
}
