using System;
using System.Collections.Generic;
using System.Text;
using Unit.DbModel.NoSql;

namespace Unit.DbModel
{
    public class WorkBeginning
    {
        public WorkBeginning(int from, DicUnit dic)
        {
            From = from;
            DicUnit = dic;
        }

        public int From { get; set; }
        /// <summary>
        /// 如果count是0就是背完了
        /// </summary>
        public DicUnit DicUnit { get; set; }
    }
}
