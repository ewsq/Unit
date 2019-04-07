using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 表示参数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple =true,Inherited =true)]
    public abstract class WithParamtersAttribute:WithOptionAttribute
    {
    }
}
