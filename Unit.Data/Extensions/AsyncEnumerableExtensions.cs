using Extensions.Reps;
using Unit.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class AsyncEnumerableExtensions
    {

        public static async Task<EnumableRep<T,RepCodes>> ToEnumableRepAsync<T>(this IAsyncEnumerable<T> enumerable,int s,int t)
        {
            var total = await enumerable.Count();
            var entities = await enumerable.Skip(s).Take(t).ToList();
            return new EnumableRep<T, RepCodes>()
            {
                IsOk = true,
                InfoType = RepCodes.Succeed,
                Entities = entities,
                Skip = s,
                Take = t
            };
        }
    }
}
