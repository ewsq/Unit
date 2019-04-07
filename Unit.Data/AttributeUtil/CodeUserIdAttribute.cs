using Unit.Data.Countermeasure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.AttributeUtil
{
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple =false,Inherited =false)]
    public class CodeUserIdAttribute : UserIdAttribute
    {
        public CodeUserIdAttribute() 
            : base("GetUserId", false, "uid")
        {
        }
    }
}
