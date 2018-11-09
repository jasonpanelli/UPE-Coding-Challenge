using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Code_Challenge
{
    public static class Dir
    {
        public const int UP = 0;
        public const int DOWN = 1;
        public const int RIGHT = 2;
        public const int LEFT = 3;
    }
    class Maze
    {
        string url, mazeInfo;
        Request Request;
        int width, height, x, y, TotalLevels, LevelsCompleted;
        string[] maze;

        public Maze(string myUrl)
        {
            url = myUrl;
            Request = new Request();
            GetMazeInfo();
            PrintMazeInfo();
            maze = new string[height];
            string fill = "";
            for (int i = 0; i < width; i++)
                fill += "?";
            for (int i = 0; i < height; i++)
                maze[i] = fill;
        }

        public bool Move(int direction)
        {
            string Result = "";
            int potentialX = x, potentialY = y;
            switch(direction)
            {
                case Dir.UP:
                    if (y - 1 < 0 || getMaze(x, y - 1) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "UP").Result);
                        potentialY = y - 1;
                    }
                    break;
                case Dir.DOWN:
                    if (y + 1 >= height || getMaze(x, y + 1) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "DOWN").Result);
                        potentialY = y + 1;
                    }
                    break;
                case Dir.LEFT:
                    if (x - 1 < 0 || getMaze(x - 1, y) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "LEFT").Result);
                        potentialX = x - 1;
                    }
                    break;
                case Dir.RIGHT:
                    if (x + 1 >= width || getMaze(x + 1, y) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "RIGHT").Result);
                        potentialX = x + 1;
                    }
                    break;
            }

            switch(Result)
            {
                case "SUCCESS":
                    x = potentialX;
                    y = potentialY;
                    break;
                case "WALL":
                    // TODO: set character at potentialX, potentialY to '*'
                    break;
                case "END":
                    break;
                case "OUT_OF_BOUNDS":
                    Console.WriteLine("ERROR: Made a post call on a bounds.");
                    break;
            }
            return false;
        }

        string GetResult(string JSON)
        {
            dynamic data = JObject.Parse(JSON);
            return data.result;
        }

        char getMaze(int xPos, int yPos)
        {
            return maze[yPos][xPos];
        }

        void GetMazeInfo()
        {
            mazeInfo = Request.GetRequest(url).Result;
            dynamic data = JObject.Parse(mazeInfo);
            width = data.maze_size[0];
            height = data.maze_size[1];
            x = data.current_location[0];
            y = data.current_location[1];
            LevelsCompleted = data.levels_completed;
            TotalLevels = data.total_levels;
        }
        void PrintMazeInfo()
        {
            Console.WriteLine("Width: " + width + "\nHeight: " + height + "\nX: " +
                x + "\nY: " + y + "\nTotal Levels: " + TotalLevels + "\nLevels Completed: " + LevelsCompleted);
        }
    }
}
