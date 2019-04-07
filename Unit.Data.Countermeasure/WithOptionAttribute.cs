using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 表示一个特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All,AllowMultiple =false,Inherited =false)]
    public abstract class WithOptionAttribute:Attribute
    {
        /// <summary>
        /// 获取一个[{*}]的方法例如返回Route("/api/v1")
        /// </summary>
        /// <returns></returns>
        protected abstract string GetAttribute();
        /// <summary>
        /// 返回$"[{GetAttribute()}]"
        /// </summary>
        /// <returns></returns>
        public string GetOption()
        {
            return $"[{GetAttribute()}]";
        }
    }
}
