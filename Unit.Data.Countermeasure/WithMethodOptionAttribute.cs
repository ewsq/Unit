using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 表示方法特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public abstract class WithMethodOptionAttribute:WithOptionAttribute
    {
    }
}
