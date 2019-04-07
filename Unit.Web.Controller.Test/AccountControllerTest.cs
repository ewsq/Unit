using Drifting.JWT.Provider;
using Extensions.Reps;
using Microsoft.AspNetCore.Mvc.Testing;
using Unit.DbModel;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Unit.Web.Controller.Test
{
    public class AccountControllerTest : AuthroizeTestBase
    {
        public AccountControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("hello123","hello123","hello123")]
        public async Task TestRegister(string userName,string pwd,string iname)
        {
            var rp =await RegisterAsync(userName, pwd, iname);
            Assert.True(rp.Entity);
        }
        [Theory]
        [InlineData("hello123", "hello123")]
        public async Task TestLogin(string userName, string pwd)
        {
            await TestRegister(userName,pwd,userName);
            var rp = await LoginAsync(userName, pwd);
            Assert.True(!string.IsNullOrEmpty(rp.Entity?.AccessToken));
        }
        [Theory]
        [InlineData("hello123", "111")]
        public async Task TestLogin_Fail(string userName, string pwd)
        {
            var context = HttpContentHelper.CreateMultipartFormDataContent((userName, "userName"), (pwd, "pwd"));
            var r = await _client.PostAsync("api/v1/account/login", context);
            var rp = await r.Content.ReadAsAsync<EntityRep<TokenEntity, RepCodes>>();
            context.Dispose();
            r.Dispose();
            context.Dispose();
            Assert.True(r.StatusCode == HttpStatusCode.OK);
            Assert.True(rp.Entity?.AccessToken==null);
        }
    }

}
