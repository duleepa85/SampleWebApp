using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Test.Factory
{
    public class Http
    {
        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }

        public static HttpRequest CreateGetRequest(Dictionary<string, StringValues> queryValues = null)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = "GET";
            if (queryValues != null)
            {
                request.Query = new QueryCollection(queryValues);
            }
            return request;
        }

        public static HttpRequest CreateDynamicRequest(string json, string reqMethod = "POST")
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = reqMethod;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            request.Body = stream;
            request.ContentLength = stream.Length;
            request.EnableBuffering();
            return request;
        }
    }
}
