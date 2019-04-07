using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 指示是post方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HttpPostMethodAttribute : HttpMethodAttribute
    {
        public static string HttpPostName = "HttpPost";
        public HttpPostMethodAttribute(string name=null) 
            : base(HttpPostName, name)
        {
        }
    }
}
