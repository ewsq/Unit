using Extensions.Reps;
using Unit.Data;
using Unit.DbModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Data.Responses
{
    public abstract class DataResponseBase : ResponseBase<RepCodes>
    {
        protected DataResponseBase(WDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public WDbContext DbContext { get;  }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="succeedCondition"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        protected Task<Rep<RepCodes>> SaveChangedAsync(Func<int, SucceedConditionResult<Rep<RepCodes>>> succeedCondition = null, Func<Exception, Rep<RepCodes>> exceptionHandler = null)
        {
            return SaveChangedAsync<Rep<RepCodes>>(succeedCondition, exceptionHandler);
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="succeedCondition"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        protected async Task<T> SaveChangedAsync<T>(Func<int, SucceedConditionResult<T>> succeedCondition = null, Func<Exception, T> exceptionHandler = null)
            where T:Rep<RepCodes>,new()
        {
            try
            {
                var sres = await DbContext.SaveChangesAsync();
                T res = null;
                if (succeedCondition!=null)
                {
                    var cdiRes = succeedCondition(sres);
                    res=cdiRes.Result?? FromCode<T>(cdiRes.Succeed ? RepCodes.Succeed : RepCodes.FailUnknow);
                }
                else
                {
                    res = FromCode<T>(RepCodes.Succeed);
                }
                return res;
                
            }
            catch (Exception ex)
            {
                return exceptionHandler?.Invoke(ex) ?? FromCode<T>(RepCodes.Exception,ex.Message);
            }
        }
        protected SucceedConditionResult<T> SucceedMoreThan<T>(int i, int target)
            where T : Rep<RepCodes>, new()
        {
            var succeed = i >= target;
            return new SucceedConditionResult<T>(succeed, FromCode<T>(succeed?RepCodes.Succeed: RepCodes.FailUnknow));
        }
    }
    /// <summary>
    /// 成功条件过滤后的结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SucceedConditionResult<T>
        where T:Rep<RepCodes>,new()
    {
        public SucceedConditionResult(bool succeed, T result)
        {
            Succeed = succeed;
            Result = result;
        }

        /// <summary>
        /// 是否成功了
        /// </summary>
        public bool Succeed { get; }
        /// <summary>
        /// 返回的结果，如果是null并且<see cref="Succeed"/>为false时返回实体代码<see cref="Rep{TCode}.InfoType=Unknow"/>，true时返回实体代码<see cref="Rep{TCode}.InfoType=Succeed"/>
        /// </summary>
        public T Result { get; }
    }
}
