using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;

namespace Code_Challenge
{
    class Request
    {
        public async void GetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        Console.WriteLine(myContent);
                    }
                }
            }
        }

        public async void PostRequest(string url, string key, string value)
        {
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
                        string myContent = await content.ReadAsStringAsync();
                        Console.WriteLine(myContent);
                    }
                }
            }
        }
    }
}
