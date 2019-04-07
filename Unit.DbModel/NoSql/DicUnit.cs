using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Unit.DbModel.NoSql
{
    /// <summary>
    /// 字典单元
    /// </summary>
    public class DicUnit : NoSqlBase
    {
        [NotMapped]
        public static readonly string DicUnitTableName = "DicUnit";
        /// <summary>
        /// 标题名字
        /// </summary>
        [BsonElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 背景图路径
        /// </summary>
        [BsonElement("img")]
        public string ImgPath { get; set; }
        [BsonElement]
        public ICollection<WordUnit> Words { get; set; }
    }
    public class WordUnit
    {
        [BsonElement("tip")]
        public string Tip;
        [BsonElement("remember")]
        public string Remember;

        public WordUnit(string tip, string remember)
        {
            Tip = tip;
            Remember = remember;
        }
    }
}
