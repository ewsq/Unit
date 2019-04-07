using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 指示控制器添加一个路由
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class WithClassRouteAttribute : WithClassOptionAttribute
    {
        public WithClassRouteAttribute(string routePath)
        {
            RoutePath = routePath;
            if (RoutePath==null)
            {
                throw new ArgumentException("路由路径不允许为空");
            }
        }

        /// <summary>
        /// 路由路径
        /// </summary>
        public string RoutePath { get; set; }
        protected override string GetAttribute()
        {
            return $"Route(\"{RoutePath}\")";
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class WithMethodRouteAttribute : WithMethodOptionAttribute
    {
        public WithMethodRouteAttribute(string routePath)
        {
            RoutePath = routePath;
            if (RoutePath == null)
            {
                throw new ArgumentException("路由路径不允许为空");
            }
        }

        /// <summary>
        /// 路由路径
        /// </summary>
        public string RoutePath { get; set; }
        protected override string GetAttribute()
        {
            return $"Route(\"{RoutePath}\")";
        }
    }
}
