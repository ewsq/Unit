
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Unit.DbModel;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Unit.Data.Test.Responses
{
    public class ResponseTestBase
    {
        protected WDbContext _dbContext { get; private set; }

        public ResponseTestBase()
        {
            
        }
        protected void Flushdb()
        {
            _dbContext?.Dispose();
            var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<WDbContext>()
                           .UseInMemoryDatabase("fwadb")
                           .UseInternalServiceProvider(serviceProvider);
            _dbContext = new WDbContext(builder.Options);
            _dbContext.Database.EnsureCreated();
            BatchUpdateManager.InMemoryDbContextFactory = () => _dbContext;
        }
        protected async Task<bool> AddUserAsync(ulong id,string name,string pwd)
        {
            var user = new SUser()
            {
                Id=id,
                NormalizedUserName=name,
                EmailConfirmed=false,
                PhoneNumberConfirmed=false,
                TwoFactorEnabled=false,
                LockoutEnabled=false,
                AccessFailedCount=0,
                IdentityName=name
            };
            _dbContext.Users.Add(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
