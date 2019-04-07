using MongoDB.Driver;
using Unit.Data.Options;
using Unit.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Responses
{

    public abstract class NoDataResponseBase : ResponseBase<RepCodes>
    {
        public static readonly string DefaultDbName = "nodb";
        protected NoDataResponseBase(NoDataOptions options)
        {
            Options = options;
            Client = new MongoClient(options.LinkSettings);
            DefaultDatabase=Client.GetDatabase(DefaultDbName);
        }
        /// <summary>
        /// 连接的设置
        /// </summary>
        public NoDataOptions Options { get; }
        /// <summary>
        /// mongo数据库连接状态
        /// </summary>
        public IMongoClient Client { get; }

        protected IMongoDatabase DefaultDatabase { get; }
    }
}
