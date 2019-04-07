using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Unit.Data.Options;
using Unit.Data.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Extensions
{
    public static class CoreServiceCollectionExtensions
    {
        public static void AddResponses(this IServiceCollection services)
        {
            services.AddTransient<UserResponse>();
            services.AddTransient<DicUnitResponse>();
        }
    }
}
