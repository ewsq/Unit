using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 表示生成的控制器是哪一个命名空间
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class WithNameSpaceAttribute : Attribute
    {
        public WithNameSpaceAttribute(string nameSpace)
        {
            NameSpace = nameSpace;
            if (string.IsNullOrEmpty(NameSpace))
            {
                throw new ArgumentNullException(nameof(nameSpace));
            }
        }
        /// <summary>
        /// 命名空间名字
        /// </summary>
        public string NameSpace { get; }
    }
}
