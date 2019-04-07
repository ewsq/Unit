using Extensions.Reps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unit.Data.Options;
using Unit.DbModel;
using Unit.DbModel.NoSql;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using Extensions.Settings;
using Unit.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MongoDB.Bson;

namespace Unit.Data.Responses
{
    public class DicUnitResponse : NoDataResponseBase
    {
        private readonly Globle globle;
        private readonly DicBckFolder dicBckFolder;
        private readonly IMongoCollection<DicUnit> DicUnits;
        private readonly WDbContext wDbContext;
        public DicUnitResponse(NoDataOptions options, Globle globle,WDbContext wDbContext) : base(options)
        {
            DicUnits = DefaultDatabase.GetCollection<DicUnit>(DicUnit.DicUnitTableName);
            this.globle = globle;
            dicBckFolder = globle.GetGlobleFolder<DicBckFolder>();
            this.wDbContext = wDbContext;
        }
        public async Task<List<DicUnit>> GetDicsAsync()
        {
            return await DicUnits.Find(f => f.Id != null).ToListAsync();
        }
        public async Task<DicPackage> GetDicAsync(string oid, ulong? uid)
        {
            var o = ObjectId.Parse(oid);
            var tot = await DicUnits.Find(c => c.Id == o).Project(d => d.Words.Count).SingleOrDefaultAsync();
            var dic = await DicUnits.Find(u => u.Id == o).Project(d => new DicUnit
            {
                Id=d.Id,
                 ImgPath=d.ImgPath,
                  Title=d.Title
            }).SingleOrDefaultAsync();
            var larst = -1;
            if (uid != null)
            {
                var to = await wDbContext.ComplateUnits.AsNoTracking()
                    .Where(c => c.UserId == uid && c.DicId == oid)
                    .OrderByDescending(c => c.To)
                    .Select(c => new { to = c.To })
                    .FirstOrDefaultAsync();
                if (to != null)
                {
                    larst = to.to;
                }
            }
            return new DicPackage(dic, larst, tot);
        }
        public async Task<Rep<RepCodes>> InsertDicAsync(string title,IFormFile bckFile,string words)
        {
            var js=JsonSerializer.CreateDefault();
            List<WordUnit> ws = null;
            words = words ?? string.Empty;
            try
            {
                using (var sr = new StringReader(words))
                using (var jr = new JsonTextReader(sr)) 
                {
                    ws = js.Deserialize<List<WordUnit>>(jr);
                }
                var fex = bckFile.FileName.Split('.').Last() ;
                var fn = Guid.NewGuid().ToString()+"."+ fex;
                var fp = Path.Combine(dicBckFolder.Directory.FullName, fn);
                using (var fs = File.Create(fp))
                {
                    await bckFile.CopyToAsync(fs);
                }
                await DicUnits.InsertOneAsync(new DicUnit()
                {
                    Title = title,
                    Words = ws,
                    ImgPath = fn
                });
                return FromCode(RepCodes.Succeed);
            }
            catch (Exception ex)
            {
                return FromCode(RepCodes.Exception, ex.Message);
            }
        }
        public async Task<EntityRep<DicPackage,RepCodes>> GetRandomDicPackageAsync(ulong? uid)
        {
            var dic = await DicUnits.Find(c => c.Id != null).CountDocumentsAsync();
            var rand = new Random();
            var us =await DicUnits.Find(f => f.Id!=null).Skip(rand.Next(0,(int)dic)).Project(p => new DicUnit()
            {
                Id = p.Id,
                ImgPath = p.ImgPath,
                Title = p.Title
            }).FirstOrDefaultAsync();
            var larst = -1;
            if (uid!=null)
            {
                var oid = us.Id.ToString();
                var to= await wDbContext.ComplateUnits.AsNoTracking().Where(c=>c.UserId==uid&&c.DicId== oid).OrderByDescending(c => c.To).Select(c=> new { to=c.To}).FirstOrDefaultAsync();
                if (to!=null)
                {
                    larst = to.to;
                }
            }
            return FromEntity(RepCodes.Succeed, new DicPackage(us,larst,dic));
        }
        public async Task<EntityRep<WorkBeginning, RepCodes>> Get10DicWordsAsync(ulong uid,string oid)
        {
            var from = 0;
            var to = await wDbContext.ComplateUnits.AsNoTracking()
                .Where(c => c.UserId == uid)
                .OrderByDescending(c => c.To)
                .Select(c => new { to = c.To })
                .FirstOrDefaultAsync();
            if (to!=null)
            {
                from = to.to;
            }
            var od = ObjectId.Parse(oid);
            var us = await DicUnits.Find(f => f.Id==od).Project(p => new DicUnit()
            {
                Id = p.Id,
                ImgPath = p.ImgPath,
                Title = p.Title,
                Words=p.Words.Skip(from).Take(10).ToArray()
            }).FirstOrDefaultAsync();
            return FromEntity(RepCodes.Succeed, new WorkBeginning(from, us));
        }
        public async Task<Rep<RepCodes>> ComplateAsync(ulong uid,int from,string oid,string info)
        {
            var d = new ComplateUnit()
            {
                UserId = uid,
                DicId = oid,
                JsonResult = info,
                From = from,
                To = from + 10
            };
            wDbContext.ComplateUnits.Add(d);
            var r=await wDbContext.SaveChangesAsync();
            return FromCode(RepCodes.Succeed);
        }
        public string GetBckFile(string fn)
        {
            return Path.Combine(dicBckFolder.Directory.FullName, fn);
        }
    }
}
