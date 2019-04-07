using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.DbModel.NoSql
{
    public abstract class NoSqlBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
