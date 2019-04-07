using Unit.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Unit.Data
{
    public class WDbContext : IdentityDbContext<SUser, SRole, ulong>
    {
        public WDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public WDbContext()
        {
        }
        public DbSet<ComplateUnit> ComplateUnits { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=47.102.142.251;Database=unitdb;User=root;Password=a1-s2=d3f4G5!;");
            //var dbc = configuration.GetSection("dbConnection").GetSection("sql");
            //var type = dbc.GetValue<string>("type");
            //switch (type)
            //{
            //    case "mysql":
            //        optionsBuilder.UseMySql(dbc.GetValue<string>("connection"));
            //        break;
            //    case "memory":
            //        optionsBuilder.UseInMemoryDatabase(dbc.GetValue<string>("tmpdbname"));
            //        break;
            //    default:
            //        optionsBuilder.UseInMemoryDatabase("tmpdb");
            //        break;
            //}
        }
    }
}
