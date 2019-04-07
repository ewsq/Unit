using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Unit.Data.Countermeasure
{

    /// <summary>
    /// 生成代码
    /// </summary>
    [AttributeUsage( AttributeTargets.Parameter,AllowMultiple =true,Inherited =true)]
    public abstract class CodeAttribute : Attribute
    {
        protected static readonly string[] EmptyParamters = Array.Empty<string>();
        protected CodeAttribute(string method, bool async,string replaceParamter,params string[] paramters)
        {
            Method = method;
            Async = async;
            ReplaceParamter = replaceParamter;
            if (ReplaceParamter!=null)
            {
                if (!Regex.IsMatch(ReplaceParamter, "^[a-zA-Z][a-zA-Z0-9]*"))
                {
                    throw new ArgumentException($"{ReplaceParamter}不合法");
                }
            }
            Paramters = paramters;
        }

        public string Method { get; }
        public bool Async { get;  }
        /// <summary>
        /// 替换参数的名字，如果为null就是不是替换
        /// </summary>
        public string ReplaceParamter { get; }
        public IEnumerable<string> Paramters { get; }
        public string GetCode()
        {
            var baseCode = $"{(Async ? "await" : string.Empty)}{Method}({string.Join(",", Paramters)});";
            if (ReplaceParamter!=null)
            {
                baseCode = $"var {ReplaceParamter}= {baseCode}";
            }
            return baseCode;
        }
    }
}
