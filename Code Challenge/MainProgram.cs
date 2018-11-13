using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Code_Challenge
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            string url = "http://ec2-34-216-8-43.us-west-2.compute.amazonaws.com/game?token=";

            Request Request = new Request();
            string tokenString = Request.PostRequest("http://ec2-34-216-8-43.us-west-2.compute.amazonaws.com/session", 
                "uid", "604967089").Result;
            Console.WriteLine(tokenString);

            dynamic data = JObject.Parse(tokenString);
            string token = data.token;
            url += token;

            Maze maze = new Maze(url);

            string temp = Request.PostRequest(url, "action", "UP").Result;
            temp = Request.PostRequest(url, "action", "LEFT").Result;
            temp = Request.PostRequest(url, "action", "RIGHT").Result;
            temp = Request.PostRequest(url, "action", "DOWN").Result;
            Console.Write("\n");
            maze.SolveAll();
            Console.ReadKey(true);
        }
    }

}
