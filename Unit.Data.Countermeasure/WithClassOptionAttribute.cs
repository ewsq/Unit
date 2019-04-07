using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 控制器特性添加器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    public abstract class WithClassOptionAttribute:WithOptionAttribute
    {

    }
}
