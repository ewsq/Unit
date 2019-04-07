using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 指示是身份验证方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AuthroizeMethodAttribute : WithMethodOptionAttribute
    {
        public AuthroizeMethodAttribute(string role=null)
        {
            Role = role;
            if (Role!=null)
            {
                parseRole = $"Policy =\"{Role}\"";
            }
            else
            {
                parseRole = string.Empty;
            }
        }
        private readonly string parseRole;
        public string Role { get; }
        protected override string GetAttribute()
        {
            return $"Authorize({parseRole})";
        }
    }
}
