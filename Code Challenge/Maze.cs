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
        Stack<int> moves;
        int width, height, x, y, TotalLevels, LevelsCompleted;
        char[][] maze;

        public Maze(string myUrl)
        {
            url = myUrl;
            Request = new Request();
            GetMazeInfo();
            PrintMazeInfo();

            moves = new Stack<int>();

            if (width > 60 || height > 60)
                Console.WriteLine("WARNING: Maze is too big. \nWidth: " + width +
                    "\nHeight: " + height);


            maze = new char[60][];
            for (int i = 0; i < 60; i++)
            {
                maze[i] = new char[60];
                for (int j = 0; j < 60; j++)
                    maze[i][j] = '?';
            }
            maze[y][x] = '-';
        }

        public void SolveAll()
        {
            while (LevelsCompleted != TotalLevels)
                Solve();
        }

        void Solve()
        {
            if (x + 1 < width && GetMaze(x+1, y) == '?')
            {
                if (Move(Dir.RIGHT))
                    return;
            }
            else if (y + 1 < height && GetMaze(x, y+1) == '?')
            {
                if (Move(Dir.DOWN))
                    return;
            }
            else if (x - 1 >= 0 && GetMaze(x-1, y) =='?')
            {
                if (Move(Dir.LEFT))
                    return;
            }
            else if (y - 1 >= 0 && GetMaze(x, y-1) == '?')
            {
                if (Move(Dir.UP))
                    return;
            }
            else
            {
                int move = moves.Pop();
                switch(move)
                {
                    case Dir.UP:
                        Move(Dir.DOWN);
                        break;
                    case Dir.DOWN:
                        Move(Dir.UP);
                        break;
                    case Dir.RIGHT:
                        Move(Dir.LEFT);
                        break;
                    case Dir.LEFT:
                        Move(Dir.RIGHT);
                        break;
                }
                moves.Pop();
            }
            //PrintMaze();
            Solve();
        }

        public bool Move(int direction)
        {
            string Result = "";
            int potentialX = x, potentialY = y;
            switch(direction)
            {
                case Dir.UP:
                    if (y - 1 < 0 || GetMaze(x, y - 1) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "UP").Result);
                        potentialY = y - 1;
                        moves.Push(Dir.UP);
                    }
                    break;
                case Dir.DOWN:
                    if (y + 1 >= height || GetMaze(x, y + 1) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "DOWN").Result);
                        potentialY = y + 1;
                        moves.Push(Dir.DOWN);
                    }
                    break;
                case Dir.LEFT:
                    if (x - 1 < 0 || GetMaze(x - 1, y) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "LEFT").Result);
                        potentialX = x - 1;
                        moves.Push(Dir.LEFT);
                    }
                    break;
                case Dir.RIGHT:
                    if (x + 1 >= width || GetMaze(x + 1, y) == '*')
                        return false;
                    else
                    {
                        Result = GetResult(Request.PostRequest(url, "action", "RIGHT").Result);
                        potentialX = x + 1;
                        moves.Push(Dir.RIGHT);
                    }
                    break;
            }

            switch(Result)
            {
                case "SUCCESS":
                    x = potentialX;
                    y = potentialY;
                    maze[y][x] = '-';
                    break;
                case "WALL":
                    maze[potentialY][potentialX] = '*';
                    moves.Pop();
                    break;
                case "END":
                    moves.Clear();
                    if (LevelsCompleted != TotalLevels - 1)
                        GetMazeInfo();
                    else
                        Console.WriteLine("Congratulations! You have solved all maze.");
                    ResetMazeData();
                    return true;
                case "OUT_OF_BOUNDS":
                    Console.WriteLine("ERROR: Made a post call on a bounds.");
                    moves.Pop();
                    break;
            }
            return false;
        }

        string GetResult(string JSON)
        {
            dynamic data = JObject.Parse(JSON);
            return data.result;
        }

        char GetMaze(int xPos, int yPos)
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

        void ResetMazeData()
        {
            for (int i = 0; i < 60; i++)
            {
                maze[i] = new char[60];
                for (int j = 0; j < 60; j++)
                    maze[i][j] = '?';
            }
        }
        public void PrintMazeInfo()
        {
            Console.WriteLine("Width: " + width + "\nHeight: " + height + "\nX: " +
                x + "\nY: " + y + "\nTotal Levels: " + TotalLevels + "\nLevels Completed: " + LevelsCompleted);
        }
        public void PrintMaze()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(maze[i][j]);
                }
                Console.Write("\n");
            }
        }
    }
}
