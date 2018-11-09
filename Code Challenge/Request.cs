using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;

namespace Code_Challenge
{
    class Request
    {
        public async Task<string> GetRequest(string url)
        {
            string returnValue;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        returnValue = await content.ReadAsStringAsync();
                        Console.WriteLine(returnValue);
                    }
                }
            }
            return returnValue;
        }

        public async Task<string> PostRequest(string url, string key, string value)
        {
            string returnValue;
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>(key,value)
            };

            HttpContent q = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(url,q))
                {
                    using (HttpContent content = response.Content)
                    {
                        returnValue = await content.ReadAsStringAsync();
                        Console.WriteLine(returnValue);
                    }
                }
            }

            return returnValue;
        }
    }
}
