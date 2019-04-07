using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 指示生成的控制器名字{*}Controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ControllerNameAttribute : Attribute
    {
        public ControllerNameAttribute(string name)
        {
            Name = name;
            if (!Regex.IsMatch(name, "^[a-zA-Z][a-zA-Z0-9]*"))
            {
                throw new ArgumentException($"{name}该名字不符合");
            }
        }
        /// <summary>
        /// 控制器名字
        /// </summary>
        public string Name { get; }
    }
}
