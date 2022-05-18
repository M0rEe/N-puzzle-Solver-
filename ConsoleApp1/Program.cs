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
            file = new FileStream("test.txt", FileMode.Open, FileAccess.Read);
            sr = new StreamReader(file);
            line = sr.ReadLine();
            size = int.Parse(line);
            int[,] goal = new int[size, size];
            line = sr.ReadLine();
            int ch = -1;
            Dictionary<int, List<string>> Row = new Dictionary<int, List<string>>();
            int indexi=0 , indexj=0;
            for (int i = 0; i < size; )
            {
                if(line == "")
                {
                    line = sr.ReadLine();
                    i = 0;
                    continue;
                }
                Row.Add(i, new List<string>());
                List<string> vertices = line.Split(' ').ToList();
                Row[i] = vertices;
                for (int j = 0; j < size; j++)
                {
                    goal[i, j] = i * size + (j+1);
                    indexj = j;
                }
                indexi = i;
                i++;
                line = sr.ReadLine();
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
            //int heuristic = 1;
            Node[,] element = new Node [size,size];
            Node startnode = new Node();
            Node End = new Node();
            BFSNode bfsend = new BFSNode();
            for (int i = 0; i < size; i++)
            {
                foreach (var el in Row[i])
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(Convert.ToUInt16(i), Convert.ToUInt16(j),UInt16.Parse(el));
                    board[i, j] = int.Parse(el);
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
            Console.Write(".");
            Console.WriteLine(".");


            Stopwatch valid = new Stopwatch();
            valid.Start();
            bool Sol = CheckSolvability(size, Row, temp_arr);
            valid.Stop();
            if (Sol)
            {
                Console.Write(">>>");
                Console.WriteLine("Solvable");
                Console.WriteLine("Time of solvability : {0}", valid.Elapsed);
                Console.WriteLine("What algorithim do you want to be used??   [0]A*   OR  [1]BFS");
                ch = int.Parse(Console.ReadLine());
                if (ch == 0) 
                {
                    
                    Console.WriteLine("What heuristic function do you want to use ?? [0]Manhattan & hamming  OR   [1]Manhattan only?  ");
                    int heuristic = int.Parse(Console.ReadLine());
                    if(heuristic != 1 && heuristic != 0)
                    {
                        Console.WriteLine("Invalid input");
                        return;
                    }
                    if(heuristic == 0 )
                        Console.WriteLine("Code will run on [0]Manhattan  then   [1]Hamming  ");
                    else
                        Console.WriteLine("Code will run on **Only Manhattan** ");
                    for (int k = 0; k < 2; k++) 
                    {
                        if (k == 0)
                            Console.WriteLine("[0]Manhattan     ");
                        if (heuristic == 1 && k == 1) break;
                        else if (k == 1) 
                            Console.WriteLine("[1]Hamming       ");
                        Stopwatch stopwatch = new Stopwatch();
                        bool ReachedGoal = false;
                        // Begin timing
                        stopwatch.Start();
                        System.Threading.Thread.Sleep(500);
                        GC.Collect();
                        Node temp = startnode.Astar(startnode, board, ref size, ref goal, k, ref ReachedGoal);//O(E Log V)
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
                        Console.WriteLine("############################################################");
                    }
                }else if (ch == 1)
                {
                    BFSNode[,] graph = new BFSNode[size, size];
                    BFSNode firstnode = new BFSNode();
                    for (int i = 0; i < size; i++)
                    {
                        foreach (var el in Row[i])
                        {
                            int j = Row[i].IndexOf(el);
                            graph[i, j] = new BFSNode(size, int.Parse(el), board)
                            {
                                Parent = null,
                                X = i,
                                Y = j,
                                
                            };
                            board[i, j] = int.Parse(el);
                            if (el.Equals("0"))
                            {
                                firstnode = graph[i, j];
                            }
                        }
                    }
                    Stopwatch bfswatch = new Stopwatch();

                    bfswatch.Start();
                    System.Threading.Thread.Sleep(500);
                    BFSNode final = firstnode.BFS(firstnode,size,goal);
                    bfsend = final;
                    bfswatch.Stop();
                    if (final != null)
                    {
                        
                        Console.WriteLine("Found the goal ");
                        Console.WriteLine("Time of solvability : {0}", bfswatch.Elapsed);
                        Console.WriteLine("board ");
                        for (int i =0;i< size;i++)
                        {
                            for (int j =0;j < size; j++)
                            {
                                Console.Write(final.board[i, j]);
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Returned null");
                    }
                }
            }
            else
            {
                Console.Write(">>>");
                Console.WriteLine("NOT Solvable");
            }
            if (Sol)
            {
                Console.WriteLine("Do you want to print all steps ??   [Y]   OR   [N]");
                string choice = Console.ReadLine();
                if (ch == 0)
                {

                    if (choice.ToLower().Equals("y"))
                    {
                        Console.WriteLine("What do you want ??   [0]Only Directions   OR   [1]Full Board");
                        int C = int.Parse(Console.ReadLine());
                        if (C != 0 && C != 1) Console.WriteLine("invalid input  ");
                        else Printpath(End,size,C);
                        Console.WriteLine();
                    }
                }else if (ch == 1)
                {
                    PrintpathBFS(bfsend, size);
                    Console.WriteLine();
                
                }
            }
            Console.WriteLine("Executed Successfully .... !!");
            Console.ReadKey();
        }
        
        static int InversionCount(int N, string[] puzzle)// O[(s)(s)] = O(S Squared)
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

        static int Blankposition(Dictionary<int, List<string>> puzzle)//O(N Squared)
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
        //O(N Squared)O[(s)(s)] = O(S Squared)
        static bool CheckSolvability(int N, Dictionary<int, List<string>> puzzle, string[] temp_puzzle)

        {
            int cnt = InversionCount(N, temp_puzzle);// O[(s)(s)] = O(S Squared)
            int row_pos = Blankposition(puzzle);//O(N Squared)

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
        public static int step = 0;

        public static bool PrintpathBFS(BFSNode end, int size)//O(S)
        {
            if (end == null) return false;
            PrintpathBFS(end.Parent, size);
            Console.WriteLine("Step # {0} ", step);
            step++;
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
            return true;
        }

        public static bool Printpath(Node end,int size,int C)//O(S)
        {
            //base case of the recursion 
            if (end == null) //O(1)
            {
                return false;
            }
            Printpath(end.Parent,size,C);// firstnode -> parent null 
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
                Console.WriteLine("Step # {0} ", end.level);//O(S)
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
