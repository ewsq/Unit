using Drifting.Jwt.Models;
using Drifting.JWT.Provider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Unit.Account.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreServiceCollectionExtensions
    {
        private static SymmetricSecurityKey GetSigningKey(string secretKey) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        /// <summary>
        /// 加入jwt验证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwt(this IServiceCollection services, Action<WebApiOptions> options = null)
        {
            var apiOptions = new WebApiOptions();
            options?.Invoke(apiOptions);
            var s = services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = apiOptions.UseHttps;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        LifetimeValidator = (before, expires, token, param) =>
                        {
                            return expires > DateTime.UtcNow;
                        },
                        RequireExpirationTime = true,
                        ValidIssuer = apiOptions.Issuer,
                        ValidAudience = apiOptions.Audience,
                        IssuerSigningKey = GetSigningKey(apiOptions.SecretKey),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                    //cfg.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        context.Token = context.Request.Query["access_token"];
                    //        if (context.Token==null)
                    //        {
                    //            var r = context.HttpContext.Request.Headers["Authorization"].ToArray();
                    //            if (r != null && r.Length > 0)
                    //            {
                    //                context.Token = r[0];
                    //            }
                    //        }
                            
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            services.Configure<WebApiSettings>(opt =>
            {
                opt.SecretKey = apiOptions.SecretKey;
                opt.Host = apiOptions.Issuer;
            });
            services.Configure<TokenProviderOptions>(o =>
            {
                o.Issuer = apiOptions.ValidIssuer;
                o.Audience = apiOptions.ValidAudience;
                o.SigningCredentials = new SigningCredentials(GetSigningKey(apiOptions.SecretKey), SecurityAlgorithms.HmacSha256);
            });
            return s;
        }
    }
}
