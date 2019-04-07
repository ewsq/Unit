using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 表示这不是一个http方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public sealed class HttpMethodIgnoreAttribute:Attribute
    {
    }
}
