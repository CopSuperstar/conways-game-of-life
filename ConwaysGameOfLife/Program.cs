using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace conways
{
    public class Program
    {
        public static void Main()
        {
            Grid grid = new Grid(25, 25);

            Console.WriteLine("Choose pattern:");
            Console.WriteLine("1. Blinker");
            Console.WriteLine("2. Glider");
            Console.WriteLine("3. Block");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    grid.LoadPattern("blinker", 10, 10);
                    break;
                case "2":
                    grid.LoadPattern("glider", 5, 5);
                    break;
                case "3":
                    grid.LoadPattern("block", 10, 10);
                    break;
            }

            while (true)
            {
                Console.SetCursorPosition(0, 0); 
                grid.Display();   
                Thread.Sleep(200);
                grid.NextGeneration();

            }


        }
    }
    
   

    public class Cell
    {
        public bool IsAlive = false;

    }

    public class Grid
    {
        private int Width;
        private int Height;

        private Cell[,] cells;
        
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    cells[x, y] = new Cell();
                }
            }
        }

        public void SetAlive(int x, int y)
        {
            if (!IsInBounds(x, y)) return;
            cells[x, y].IsAlive = true;
        }
        public int FindNeighbors(int x, int y)
        {

            int neighborCount = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (IsInBounds(checkX, checkY) && cells[checkX, checkY].IsAlive)
                    {
                        neighborCount++;
                    }
                }
            }

            return neighborCount;
        }

        bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;

        }

        public void NextGeneration()
        {
            Cell[,] newCells = new Cell[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    newCells[x, y] = new Cell();
                    newCells[x, y].IsAlive = cells[x, y].IsAlive;
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbors = FindNeighbors(x, y);

                    if (cells[x, y].IsAlive)
                    {
                        if (neighbors < 2 || neighbors > 3)
                        {
                            newCells[x, y].IsAlive = false;
                        }
                    }
                    else
                    {
                        if (neighbors == 3)
                        {
                            newCells[x, y].IsAlive = true;
                        }
                    }
                }
            }
            cells = newCells;
        }
        public void Display()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (cells[x, y].IsAlive)
                    {
                        Console.Write(" * ");
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                Console.WriteLine();
            }
        }
        public void LoadPattern(string patternName, int startX, int startY)
        {
            switch (patternName.ToLower())
            {
                case "blinker":
                    SetAlive(startX, startY);
                    SetAlive(startX + 1, startY);
                    SetAlive(startX + 2, startY);
                    break;

                case "glider":
                    SetAlive(startX, startY + 1);
                    SetAlive(startX + 1, startY + 2);
                    SetAlive(startX + 2, startY);
                    SetAlive(startX + 2, startY + 1);
                    SetAlive(startX + 2, startY + 2);
                    break;

                case "block":
                    SetAlive(startX, startY);
                    SetAlive(startX + 1, startY);
                    SetAlive(startX, startY + 1);
                    SetAlive(startX + 1, startY + 1);
                    break;

                default:
                    Console.WriteLine("Unknown pattern");
                    break;
            }
        }
    }
}