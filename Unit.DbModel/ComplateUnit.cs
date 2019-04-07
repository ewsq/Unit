using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace Unit.DbModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ComplateUnit : IdentityDbModelBase
    {
        public ComplateUnit()
        {
        }
        /// <summary>
        /// 字典id
        /// </summary>
        [Required]
        public string DicId { get; set; }

        public int From { get; set; }
        public int To { get; set; }
        /// <summary>
        /// json List<ComplateUnti>
        /// </summary>
        public string JsonResult { get; set; }
        [Required]
        [ForeignKey("User")]
        public ulong UserId { get; set; }
        public SUser User { get; set; }
        /// <summary>
        /// 完成的数量
        /// </summary>
        [NotMapped]
        public int ComplatedCount => To - From;
        [NotMapped]
        public List<ComplateUnti> Result
        {
            get
            {
                if (JsonResult==null)
                {
                    return new List<ComplateUnti>();
                }
                List<ComplateUnti> cus = null;
                using (var sr = new StringReader(JsonResult))
                using (var tr = new JsonTextReader(sr)) 
                {
                    cus = JsonSerializer.CreateDefault().Deserialize<List<ComplateUnti>>(tr);
                }
                return cus;
            }
            set
            {
                if (value==null)
                {
                    JsonResult = string.Empty;
                    return;
                }
                var sb = new StringBuilder();
                using (var sr=new StringWriter(sb))
                {
                    JsonSerializer.CreateDefault().Serialize(sr ,value);
                }
                JsonResult = sb.ToString();
            }
        }
    }
    public struct ComplateUnti
    {
        /// <summary>
        /// from+offset就是那个单词的位置
        /// </summary>
        public int Offset;
        /// <summary>
        /// 忘记了多少次
        /// </summary>
        public ushort ForgetCount;

        public ComplateUnti(int offset, ushort forgetCount)
        {
            Offset = offset;
            ForgetCount = forgetCount;
        }
    }
}
