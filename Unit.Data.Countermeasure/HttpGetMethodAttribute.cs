using System;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 指示是get方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HttpGetMethodAttribute : HttpMethodAttribute
    {
        public static string HttpGetName = "HttpGet";
        public HttpGetMethodAttribute(string name = null)
            : base(HttpGetName, name)
        {
        }
    }
}
