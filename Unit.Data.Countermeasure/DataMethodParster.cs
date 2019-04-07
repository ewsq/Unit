using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Data.Countermeasure
{
    /// <summary>
    /// 对数据方法进行解析
    /// </summary>
    public class DataMethodParster
    {
        /// <summary>
        /// 开始生成器
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="name">控制器名字{name}Controller，如果为空，则会过滤掉Response</param>
        public ControllerBuilder Begin(Type responseType)
        {
            if (responseType==null)
            {
                throw new ArgumentNullException(nameof(responseType));
            }
            return new ControllerBuilder(responseType);
        }
        /// <summary>
        /// 控制器生成者
        /// </summary>
        
    }
    public class ControllerBuilder
    {
        public ControllerBuilder(Type responseType)
        {
            ResponseType = responseType;
            MethodInfos = ResponseType.GetMethods(BindingFlags.Instance|~BindingFlags.GetProperty|~BindingFlags.SetProperty).Where(m => m.IsPublic&&
                m.GetCustomAttribute<HttpMethodIgnoreAttribute>() == null);
        }
        /// <summary>
        /// 响应类型
        /// </summary>
        public Type ResponseType { get; }
        /// <summary>
        /// 响应公开并且没有忽略的方法
        /// </summary>
        public IEnumerable<MethodInfo> MethodInfos { get; }
        public ClassUnit Parse(bool earseAsync = true)
        {
            var nameSpaceName = ResponseType.GetCustomAttribute<WithNameSpaceAttribute>()?.NameSpace;
            if (nameSpaceName == null)
            {
                throw new ArgumentNullException("类型不存在特性WithNameSpaceAttribute");
            }
            var classOpts = ResponseType.GetCustomAttributes<WithClassOptionAttribute>();
            var classOptions = new List<string>(classOpts != null ? classOpts.Count() : 5);
            if (classOpts != null)
            {
                classOptions.AddRange(classOpts.Select(c => c.GetOption()));
            }
            var controllerName = ResponseType.GetCustomAttribute<ControllerNameAttribute>()?.Name;
            if (controllerName == null)
            {
                controllerName = ResponseType.Name.Replace("Response", "Controller");
            }
            var methodUnits = new List<MethodUnit>(MethodInfos.Count());
            foreach (var item in MethodInfos)
            {
                var name = item.Name;
                if (earseAsync && name != "Async")
                {
                    name = name.Replace("Async", string.Empty);
                }
                var options = item.GetCustomAttributes<WithMethodOptionAttribute>().Select(m => m.GetOption());
                var pars = item.GetParameters();
                var isAsync = item.GetCustomAttribute<AsyncStateMachineAttribute>()!=null;
                var pus = new List<ParamterUnit>(pars.Length);
                foreach (var p in pars)
                {
                    var parName = p.Name;
                    var parType = p.ParameterType.Name;
                    if (p.ParameterType.IsGenericType&&p.ParameterType.GetGenericTypeDefinition()==(typeof(Nullable<>)))
                    {
                        parType = p.ParameterType.GetGenericArguments()[0].Name+"?";
                    }
                    if (p.HasDefaultValue)
                    {
                        if (p.ParameterType.IsClass)
                        {
                            parName += "=null";
                        }
                        else
                        {
                            parName += $"={p.DefaultValue}";
                        }
                    }
                    var parOptions = p.GetCustomAttributes<WithParamtersAttribute>().Select(par => par.GetOption());
                    pus.Add(new ParamterUnit(parName, parType, parOptions));
                }
                var codes = pars.SelectMany(p => p.GetCustomAttributes<CodeAttribute>());
                methodUnits.Add(new MethodUnit(name, isAsync, options, codes, pus));
            }
            return new ClassUnit(ResponseType.Name,nameSpaceName, controllerName, classOptions, methodUnits);
        }
    }
    public class ClassUnit
    {
        public ClassUnit(string responseName,string nameSpaceName,string name, IEnumerable<string> classOptions, IEnumerable<MethodUnit> methodUnits)
        {
            ResponseName = responseName;
            NameSpaceName = nameSpaceName;
            Name = name;
            ClassOptions = classOptions;
            MethodUnits = methodUnits;
        }
        public string ResponseName { get; }
        public string NameSpaceName { get; }
        public string Name { get; }
        public IEnumerable<string> ClassOptions { get; }
        public IEnumerable<MethodUnit> MethodUnits { get; }
    }
    public class MethodUnit
    {
        public MethodUnit(string name,bool isAsync, IEnumerable<string> options,IEnumerable<CodeAttribute> codes, IEnumerable<ParamterUnit> paramterUnits)
        {
            Name = name;
            IsAsync = isAsync;
            Codes = codes;
            Options = options;
            ParamterUnits = paramterUnits;
        }

        public string Name { get; }
        public bool IsAsync { get; }
        public IEnumerable<CodeAttribute> Codes { get; }
        public IEnumerable<string> Options { get; }
        public IEnumerable<ParamterUnit> ParamterUnits { get; }
    }
    public class ParamterUnit
    {
        public ParamterUnit(string name, string typeName, IEnumerable<string> options)
        {
            Name = name;
            TypeName = typeName;
            Options = options;
        }

        public string Name { get; }
        public string TypeName { get; }
        public IEnumerable<string> Options { get; }

    }
}
