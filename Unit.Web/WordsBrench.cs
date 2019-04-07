using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unit.Data.Options;
using Unit.Data.Responses;
using Unit.DbModel.NoSql;

namespace Unit.Web
{
    public class WordsBrench
    {
        public List<(string, string)> ws=new List<(string, string)>();
        public WordsBrench(NoDataOptions options)
        {
            string w;
            using (var fs = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "words.txt")))
                using(var sr=new StreamReader(fs,Encoding.UTF8))
            {
                w=sr.ReadToEnd();
            }
            var ls = w.Split('\n');
            foreach (var item in ls)
            {
                var a = item.Split(" ");
                ws.Add((a[0].Trim(), a[a.Length-1].Trim()));
            }
            //var wss = new List<WordUnit>(ls.Length / 2);
            //foreach (var item in ls)
            //{
            //    if (!string.IsNullOrEmpty(item))
            //    {
            //        var s = item.Split("/");
            //        if (s.Length>1)
            //        {
            //            wss.Add(new WordUnit(s[0], s[1].Trim()));
            //        }
            //    }
            //}
            //var du = new DicUnit()
            //{
            //    Title = "日语",
            //    Words = wss
            //};
            //var Client = new MongoClient(options.LinkSettings);
            //var DefaultDatabase = Client.GetDatabase(NoDataResponseBase.DefaultDbName);
            //var d = DefaultDatabase.GetCollection<DicUnit>(DicUnit.DicUnitTableName);
            //d.InsertOne(du);

        }
        public async Task<List<(string, string)>> GetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new List<(string, string)>();
            }
            var sk = key.ToLower();
            return await ws.ToAsyncEnumerable().Where(w => w.Item1.ToLower().Contains(sk)).Take(5).ToList();
        }
    }
}
