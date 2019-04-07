using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.DbModel
{
    /// <summary>
    /// 数据库模型基类
    /// </summary>
    public abstract class DbModelBase
    {
        public DbModelBase()
        {
            Enable = true;
            CreateTime = DateTime.Now.Ticks;
        }
        /// <summary>
        /// 记录创建的时间，默认本地时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 该记录是否可用,默认true
        /// </summary>
        public bool Enable { get; set; }

    }
}
