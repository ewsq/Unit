using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Test
{
    public class SDbContextTest
    {
        public static WDbContext CreateDatabase(string dbName=null)
        {
            var options = new DbContextOptionsBuilder<WDbContext>()
                .UseInMemoryDatabase(dbName??"db")
                .Options;
            return new WDbContext(options);
        }
        [Test]
        public void TestCreate()
        {
            var res = false;
            using (var db=CreateDatabase())
            {
                res=db.Database.EnsureCreated();
            }
            Assert.True(res);
        }
        [Test]
        public void TestDrop()
        {
            var res = false;
            using (var db=CreateDatabase())
            {
                db.Database.EnsureCreated();
                res = db.Database.EnsureDeleted();
            }
            Assert.True(res);
        }
    }
}
