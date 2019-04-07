using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class UserIdAttribute : CodeAttribute
    {
        public UserIdAttribute(string method, bool async, string replaceParamter) 
            : base(method, async, replaceParamter, EmptyParamters)
        {
        }
    }
}
