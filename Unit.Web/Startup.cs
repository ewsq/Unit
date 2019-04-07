using Drifting.JWT.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Unit.Data;
using Unit.Data.Extensions;
using Unit.Data.Options;
using Unit.DbModel;
using System;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Extensions.Settings;
using Unit.Storage;
using Unti.Storage;
using System.Threading.Tasks;
using Unit.DbModel.NoSql;
using Unit.Data.Responses;

namespace Unit.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public readonly string Key = Guid.NewGuid().ToString();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/png", "image/jpg", "text/json" });
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddDbContext<WDbContext>();
            services.AddDefaultIdentity<SUser>(options =>
            {
                options.Password.RequireDigit = options.Password.RequireLowercase = options.Password.RequireNonAlphanumeric = options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddRoles<SRole>()
                 .AddEntityFrameworkStores<WDbContext>();
            var globle = new Globle();
            globle.RegisterFolder<HeadPortraitFolder>();
            globle.RegisterFolder<DicBckFolder>();
            services.AddSingleton(globle);
            var noSqlOpt = new NoDataOptions(MongoClientSettings.FromUrl(new MongoUrl(Configuration["dbConnection:nosql:connection"])));
            services.AddSingleton(noSqlOpt);
            services.AddSingleton(new WordsBrench(noSqlOpt));

            services.AddResponses();
            var issuer = Configuration["issuer"];
            services.AddJwt(options =>
            {
                options.UseHttps = false;
                options.SecretKey = Key;
                options.Issuer = issuer;
                options.Audience = issuer;
                options.ValidIssuer = issuer;
                options.ValidAudience = issuer;
            });
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var op = new TokenProviderOptions()
            {
                Issuer = issuer,
                Audience = issuer,
                ValidFor = TimeSpan.FromHours(6),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
            services.AddSingleton(op);
            services.AddSingleton(new STokenProvider(op));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder => builder.RequireClaim("Role"));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://localhost:4200/api/v1").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            //Task.Run(() =>
            //{
            //    var b = new WordsBrench(noSqlOpt);
            //    var du = new DicUnit()
            //    {
            //        Title = "sd",
            //        Words = b.ws.Select(wx => new WordUnit(wx.Item1, wx.Item2)).ToArray()
            //    };
            //    var Client = new MongoClient(noSqlOpt.LinkSettings);
            //    var DefaultDatabase = Client.GetDatabase(NoDataResponseBase.DefaultDbName);
            //    var d = DefaultDatabase.GetCollection<DicUnit>(DicUnit.DicUnitTableName);

            //    d.InsertOne(du);
            //}).Wait();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseCors();
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            using (var db=new WDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
