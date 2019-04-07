using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Unit.DbModel.NoSql;

namespace Unit.DbModel
{
    [NotMapped]
    public class DicPackage
    {
        public DicPackage(DicUnit dic, int larstIndex, long totalDicCount)
        {
            Dic = dic;
            LarstIndex = larstIndex;
            TotalDicCount = totalDicCount;
        }

        public DicUnit Dic { get;  }

        public int LarstIndex { get; }
        public long TotalDicCount { get; }
    }
}
