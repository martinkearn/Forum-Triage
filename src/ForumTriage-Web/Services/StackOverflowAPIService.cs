using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ForumTriage_Web.Services
{
    public static class StackOverflowAPIService
    {
        public static async Task<IList<JToken>> CallApi(string url)
        {
            IList<JToken> results = null;

            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var httpClient = new HttpClient(handler))
            {
                var apiUrl = (string.Format("{0}{1}", Constants.Constants.StackOverflowApiRootUrl, url));

                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request
                var responseString = await httpClient.GetStringAsync(apiUrl);

                //parse json string to object
                JObject response = JObject.Parse(responseString);
                results = response["items"].Children().ToList();
            }

            return results;
        }
    }
}
