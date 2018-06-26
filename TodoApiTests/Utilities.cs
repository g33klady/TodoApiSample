using System;
using System.Net.Http;
using System.Text;

namespace TodoApiTests
{
    public static class Utilities
    {
        /// <summary>
        /// Method that actually sends the HTTP request for Integration Tests ONLY
        /// </summary>
        /// <param name="url">full URL for the request</param>
        /// <param name="method">GET, POST, PUT, or DELETE</param>
        /// <param name="content">request body (optional)</param>
        /// <returns>HttpResponseMessage (status code and data of the http response)</returns>
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

        /// <summary>
        /// Method to read the Content of the HttpResponseMessage (result of SendHttpWebRequest)
        /// </summary>
        /// <param name="httpResponseMessage">result of SendHttpWebRequest method</param>
        /// <returns>response body as a string</returns>
        public static string ReadWebResponse(HttpResponseMessage httpResponseMessage)
        {
            using (httpResponseMessage)
            {
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
