using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Extensions.Reps;
namespace Unit.Data
{
    /// <summary>
    /// 响应的基类，存在一定的共享方法
    /// </summary>
    public abstract class ResponseBase<RepCode>
    {
        /// <summary>
        /// 组成一个返回码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public T FromCode<T>(RepCode code,string msg=null)
            where T:Rep<RepCode>,new ()
        {
            return new T()
            {
                InfoType = code,
                IsOk = true,
                Msg = msg
            };
        }
        /// <summary>
        /// 组成一个返回码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected Rep<RepCode> FromCode(RepCode code, string msg = null)
        {
            return FromCode<Rep<RepCode>>(code,msg);
        }
        /// <summary>
        /// 生成一个数据实体的返回实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="code"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected EntityRep<TEntity,RepCode> FromEntity<TEntity>(RepCode code, TEntity entity)
        {
            return new EntityRep<TEntity, RepCode>()
            {
                Entity = entity,
                InfoType = code,
                IsOk = true
            };
        }
    }
}
