using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;

namespace Code_Challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetRequest("http://ec2-34-216-8-43.us-west-2.compute.amazonaws.com/session?=604967089");
            PostRequest("http://ec2-34-216-8-43.us-west-2.compute.amazonaws.com/session");

            Console.ReadKey(true);
        }

        async static void GetRequest(string url)
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

        async static void PostRequest(string url)
        {
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("uid","604967089")
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
