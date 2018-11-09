using System;
using System.Collections.Generic;
using System.Text;

namespace Code_Challenge
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            Request request = new Request();
            request.PostRequest("http://ec2-34-216-8-43.us-west-2.compute.amazonaws.com/session", "uid", "604967089");



            Console.ReadKey(true);
        }
    }

}
