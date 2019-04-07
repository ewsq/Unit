using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Account.Options
{
    public class WebApiOptions
    {
        public WebApiOptions(Action<AuthorizationOptions> policyBuilder = null)
        {
            PolicyBuilder = policyBuilder;
            if (PolicyBuilder == null)
            {
                PolicyBuilder = options =>
                {

                    //options.AddPolicy(ClaimTypes.Role, policy => policy.RequireClaim(ClaimTypes.Name, "logined"));
                    // options.AddPolicy("Bearer",new AuthorizationPolicyBuilder());
                };
            }
        }

        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool UseHttps { get; set; }
        public string ValidIssuer { get; set; }
        /// <summary>
        /// 你的网站
        /// </summary>
        public string ValidAudience { get; set; }
        /// <summary>
        /// 创建身份管理时要用
        /// </summary>
        public Action<AuthorizationOptions> PolicyBuilder { get; }

    }

}
