using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// http方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public abstract class HttpMethodAttribute : WithMethodOptionAttribute
    {
        private static readonly string DefaultMethodName = "[action]";

        protected HttpMethodAttribute(string methodName, string name)
        {
            MethodName = methodName;
            Name = name;
        }

        /// <summary>
        /// http方法名，如HttpGet,HttpPost
        /// </summary>
        public string MethodName { get; }
        /// <summary>
        /// 具体访问名，不填就是[action]
        /// </summary>
        public string Name { get;  }
        protected sealed override string GetAttribute()
        {
            return $"{MethodName}(\"{Name??DefaultMethodName}\")";
        }

    }
}
