using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Tests
{
    public static class Utilities
    {
        public static HttpResponseMessage SendHttpWebRequest(string url, string method, string content = null)
        {
            using (var httpClient = new HttpClient())
            {
                var httpMethod = new HttpMethod(method);

                using (var httpRequestMessage = new HttpRequestMessage { RequestUri = new Uri(url), Method = httpMethod })
                {
                    if (httpMethod != HttpMethod.Get && content != null)
                    {
                        httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    }

                    return httpClient.SendAsync(httpRequestMessage).Result;
                }
            }
        }

        public static string ReadWebResponse(HttpResponseMessage httpResponseMessage)
        {
            using (httpResponseMessage)
            {
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
