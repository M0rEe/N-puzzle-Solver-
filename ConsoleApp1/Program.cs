using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream file;
            int size;
            StreamReader sr;
            string line;
            TextReader origConsole = Console.In;

            file = new FileStream("C:/Users/EGYPT/source/repos/N-puzzle-Solver-/Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/99 Puzzle - 1.txt", FileMode.Open, FileAccess.Read);

            sr = new StreamReader(file);
            line = sr.ReadLine();
            size = int.Parse(line);
            line = sr.ReadLine();
            int[,] goal = new int[size, size];

            Dictionary<int, List<string>> Row = new Dictionary<int, List<string>>();
            int indexi=0 , indexj=0;
            for (int i = 0; i < size; i++)
            {
                line = sr.ReadLine();
                Row.Add(i, new List<string>());
                List<string> vertices = line.Split(' ').ToList();
                Row[i] = vertices;
                for (int j = 0; j < size; j++)
                {
                    goal[i, j] = i * size + (j+1);
                    indexj = j;
                }
                indexi = i;
            }
            
            goal[indexi, indexj] = 0;
            sr.Close();
            file.Close();


            //solavable or 
            int cnt = 0;
            string[] temp_arr = new string[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp_arr[cnt] = Row[i][j];
                    cnt++;
                }
            }

            int[,] board  =new int[size,size];
            int heuristic = 1;
            Node[,] element = new Node [size,size];
            Node startnode = new Node(9, 9, 9);
            Node End = new Node(); 
            for (int i = 0; i < size; i++)
            {
                foreach (var el in Row[i])
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(i,j,int.Parse(el));
                    board[i, j] = int.Parse(el);
                    element[i, j].goal = goal;
                    if (el.Equals("0"))
                    {
                        startnode = element[i,j];
                    }
                }
            }

            System.Threading.Thread.Sleep(500);
            Console.Write("Checking For solvability of the input ");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(600);
            Console.Write(".");
            System.Threading.Thread.Sleep(700);
            Console.WriteLine(".");


            Stopwatch valid = new Stopwatch();
            valid.Start();
            bool x = CheckSolvability(size, Row, temp_arr);
            valid.Stop();
            if (x)
            {
                Console.Write(">>>");
                Console.WriteLine("Solvable");
                Console.WriteLine("What heuristic function do you want to use ?? [0] Manhattan  OR   [1] Hamming  ");
                heuristic = int.Parse(Console.ReadLine());
                if(heuristic != 1 && heuristic != 0)
                {
                    Console.WriteLine("Invalid input");
                    return;
                }
                Stopwatch stopwatch = new Stopwatch();
                // Begin timing
                stopwatch.Start();

                System.Threading.Thread.Sleep(500);
                PriorityQ Astarlist = new PriorityQ();
                Console.WriteLine("Start node at x,y " + startnode.X + " " + startnode.Y);
                startnode.board = board;
                startnode.goal = goal;
                startnode.G = 0;
                startnode.CalcH(heuristic, size);
                startnode.CalcF();
                startnode.level = 0;
                startnode.Parent = null;
                Astarlist.enqueue(startnode);
                Node temp = new Node();
                bool ReachedGoal = false;

                while (!Astarlist.empty())
                {
                    temp = Astarlist.dequeue();
                    //if the heuristic value to the peek node is 0 then we reached our goal 
                    if (temp.H == 0)
                    { 
                        ReachedGoal = true;
                        Console.WriteLine("Found the goal ");
                        break;
                    }
                    temp.GetAdjecents(size,Astarlist,heuristic);

                    temp.Adjecants = null;
                    temp = null;
                    
                }
                stopwatch.Stop();


                if (!ReachedGoal)
                {
                    Console.WriteLine("out of the while ");
                }
                else
                {
                    Console.Write("# of movements ");
                    Console.Write(temp.level);
                    End = temp;
                    Console.WriteLine();
                    Console.WriteLine("Time : {0}", stopwatch.Elapsed);
                }
            }
            else
            {
                Console.Write(">>>");
                Console.WriteLine("NOT Solvable");
            }


            Console.WriteLine("Time of solvability : {0}", valid.Elapsed);
            Console.WriteLine("Do you want to print all steps ??   [Y]   OR   [N]");
            string choice = Console.ReadLine();


            if (choice.ToLower().Equals("y"))
            {
                Console.WriteLine("What do you want ??   [0]Only Directions   OR   [1]Full Board");
                int C = int.Parse(Console.ReadLine());
                Printpath(End,size,C);
                Console.WriteLine();
            }
            Console.WriteLine("Executed Successfully .... !!");
        }

        static int InversionCount(int N, string[] puzzle)
        {
            int cnt = 0;
            for (int i = 0; i < N * N - 1; i++)
            {
                for (int j = i + 1; j < N * N; j++)
                {

                    int x = int.Parse(puzzle[i]);
                    int y = int.Parse(puzzle[j]);

                    if (x == 0 || y == 0)
                        continue;
                    if (x > y)
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }

        static int Blankposition(Dictionary<int, List<string>> puzzle)
        {
            //string blank = "0";

            for (int i = puzzle.Count - 1; i >= 0; i--)
            {
                for (int j = puzzle.Count - 1; j >= 0; j--)
                {
                    if (puzzle[i][j].Equals("0"))
                    {
                        return puzzle.Count - i;
                    }
                }
            }
            return 0;
        }

        static bool CheckSolvability(int N, Dictionary<int, List<string>> puzzle, string[] temp_puzzle)
        {
            int cnt = InversionCount(N, temp_puzzle);
            int row_pos = Blankposition(puzzle);

            if (N % 2 == 0)
            {
                if ((row_pos % 2 == 0 && cnt % 2 != 0))
                    return true;
                if ((row_pos % 2 != 0 && cnt % 2 == 0))
                    return true;
            }
            else
            {
                if (cnt % 2 == 0)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        public static bool Printpath(Node end,int size,int C)
        {
            //base case of the recursion 
            if (end == null)
            {
                return false;
            }
            Printpath(end.Parent,size,C);
            if (C == 0 ) 
            {
                if (end.level == 0) end.direction = 4;
                //down 0 , up 1 ,right 2 , left 3
                switch (end.direction) {
                    case 0:
                        Console.Write("D");
                        break;
                    case 1:
                        Console.Write("U");
                        break;
                    case 2:
                        Console.Write("R");
                        break;
                    case 3:
                        Console.Write("L");
                        break;
                    default:
                        break;
                }
            }
            else if(C== 1)
            {
                Console.WriteLine("Step # {0} ", end.level);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Console.Write(end.board[i, j]);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }

            return true;
        }
    }
}
