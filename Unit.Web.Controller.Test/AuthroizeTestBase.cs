using Drifting.JWT.Provider;
using Extensions.Reps;
using Microsoft.AspNetCore.Mvc.Testing;
using Unit.DbModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Unit.Web.Controller.Test
{
    public abstract class AuthroizeTestBase : AuthroizeTestBase<CustomWebApplicationFactory<Startup>>
    {
        public AuthroizeTestBase(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public AuthroizeTestBase(HttpClient client) : base(client)
        {
        }
    }
    public abstract class AuthroizeTestBase<T> : IClassFixture<T>
        where T: WebApplicationFactory<Startup>
    {
        protected TokenEntity token { get; private set; }
        protected virtual HttpClient _client { get; set; }
        protected T _factory;

        public AuthroizeTestBase(
            T factory)
        {
            _factory = factory;
            factory.ClientOptions.BaseAddress = new Uri("http://loaclhost:5000/");
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
           });
        }
        public AuthroizeTestBase(HttpClient client)
        {
            _client = client;
        }
        protected async Task<TokenEntity> LoginTestWithAuthroizeAsync()
        {
            var rand = new Random();
            var w = rand.Next(1000, 12312000);
            var context = HttpContentHelper.CreateMultipartFormDataContent(($"Hello{w}", "userName"), ($"Hello{w}", "pwd"), ($"Hello{w}", "iname"));
            var r = await _client.PostAsync("api/v1/account/register", context);
            var rp = await r.Content.ReadAsAsync<EntityRep<bool, RepCodes>>();
            context.Dispose();
            r.Dispose();
            context = HttpContentHelper.CreateMultipartFormDataContent(($"Hello{w}", "userName"), ($"Hello{w}", "pwd"));
            r = await _client.PostAsync("api/v1/account/login", context);
            var x = await r.Content.ReadAsAsync<EntityRep<TokenEntity, RepCodes>>();
            context.Dispose();
            r.Dispose();
            context.Dispose();
            Assert.True(!string.IsNullOrEmpty(x.Entity?.AccessToken));
            token = x.Entity;
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token.AccessToken);
            return x.Entity;
        }
        protected Task<T> Post<T>(string url, params (object, object)[] forms)
        {
            return Http<T>(c => _client.PostAsync(url, c), forms);
        }
        protected Task<T> Get<T>(string url)
        {
            return Http<T>(c => _client.GetAsync(url));
        }
        protected async Task<T> Http<T>(Func<HttpContent,Task<HttpResponseMessage>> func,params (object,object)[] forms)
        {
            HttpContent content=null;
            if (forms!=null)
            {
                content = HttpContentHelper.CreateMultipartFormDataContent(forms);
            }
            var r = await func(content);
            if (!r.IsSuccessStatusCode)
            {
                return default;
            }
            T rp = default;
            try
            {
                rp = await r.Content.ReadAsAsync<T>();
                content?.Dispose();
            }
            catch (Exception)
            {
                return default;
            }
            finally
            {
                r.Dispose();
            }
            return rp;
        }
        protected async Task<EntityRep<bool, RepCodes>> RegisterAsync(string userName, string pwd, string iname)
        {
            var context = HttpContentHelper.CreateMultipartFormDataContent((userName, "userName"), (pwd, "pwd"), (iname, "iname"));
            var r = await _client.PostAsync("api/v1/account/register", context);
            var rp = await r.Content.ReadAsAsync<EntityRep<bool, RepCodes>>();
            context.Dispose();
            r.Dispose();
            Assert.True(r.StatusCode == HttpStatusCode.OK);
            return rp;
        }
        protected async Task<EntityRep<TokenEntity,RepCodes>> LoginAsync(string userName, string pwd)
        {
            var context = HttpContentHelper.CreateMultipartFormDataContent((userName, "userName"), (pwd, "pwd"));
            var r = await _client.PostAsync("api/v1/account/login", context);
            var rp = await r.Content.ReadAsAsync<EntityRep<TokenEntity, RepCodes>>();
            context.Dispose();
            r.Dispose();
            context.Dispose();
            Assert.True(r.StatusCode == HttpStatusCode.OK);
            return rp;
        }
    }
}
