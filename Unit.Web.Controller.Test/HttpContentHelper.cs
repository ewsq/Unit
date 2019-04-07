using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Unit.Web.Controller.Test
{
    public static class HttpContentHelper
    {

        public static MultipartFormDataContent CreateMultipartFormDataContent(params (object,object)[] forms)
        {
            var f = new MultipartFormDataContent();
            foreach (var item in forms)
            {
                f.Add(new StringContent(item.Item1?.ToString()??string.Empty), item.Item2.ToString());
            }
            return f;
        }
    }
}
