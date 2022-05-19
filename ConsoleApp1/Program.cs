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
            //file reader info

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
            int ch = -1; //input choice 
            //N puzzle data declaration
            Dictionary<int, List<string>> Row = new Dictionary<int, List<string>>();
            int indexi = 0, indexj = 0;
            for (int i = 0; i < size;) // iterations*body--> size*size--> O(size squared)
            {
                if (line == "")
                {
                    line = sr.ReadLine();
                    i = 0;
                    continue;
                }
                //Read Npuzzle data   
                Row.Add(i, new List<string>()); //indicate each row_index 
                List<string> vertices = line.Split(' ').ToList(); //Data in each row
                Row[i] = vertices;
                //calculate Goal State
                for (int j = 0; j < size; j++) //O(size)
                {
                    goal[i, j] = i * size + (j + 1);
                    indexj = j;
                }
                indexi = i;
                i++;
                line = sr.ReadLine();
            }

            goal[indexi, indexj] = 0; // add blank space to goal state
            sr.Close();
            file.Close();

            //check Solvability without Solving Npuzzle
            int cnt = 0, indexerofzero = -1;
            string[] temp_arr = new string[size * size];
            //Switch 2D array"Dictionary" to 1D array
            for (int i = 0; i < size; i++) //iterations*body-> size*size-> O(size squared)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Row[i][j] == "0")
                        indexerofzero = i;
                    temp_arr[cnt] = Row[i][j];
                    cnt++;
                }
            }

            int[,] board = new int[size, size];
            //int heuristic = 1;
            //Node Declaration
            Node[,] element = new Node[size, size];
            Node startnode = new Node();
            Node End = new Node();
            BFSNode bfsend = new BFSNode();
            //Extract Start Node
            for (int i = 0; i < size; i++) //iterations*body-> size*size-> O(size squared)
            {
                foreach (var el in Row[i]) //O(size)
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(Convert.ToUInt16(i), Convert.ToUInt16(j), UInt16.Parse(el));
                    board[i, j] = int.Parse(el);
                    if (el.Equals("0"))
                    {
                        startnode = element[i, j];
                    }
                }
            }

            // for better display 
            System.Threading.Thread.Sleep(500);
            Console.Write("Checking For solvability of the input ");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            Console.Write(".");
            Console.WriteLine(".");


            Stopwatch valid = new Stopwatch();
            valid.Start();
            bool Sol = CheckSolvability(size, Row, temp_arr, indexerofzero); // check if solvable or not  O(s square)
            valid.Stop();
            if (Sol) // Start to solve our N puzzle 
            {
                Console.Write(">>>");
                Console.WriteLine("Solvable");
                Console.WriteLine("Time of solvability : {0}", valid.Elapsed);
                Console.WriteLine("What algorithim do you want to be used??   [0]A*   OR  [1]BFS");
                ch = int.Parse(Console.ReadLine());

                if (ch == 0) // A* Logic
                {
                    Console.WriteLine("What heuristic function do you want to use ?? [0]Manhattan & hamming  OR   [1]Manhattan only?  ");
                    int heuristic = int.Parse(Console.ReadLine());
                    if (heuristic != 1 && heuristic != 0)
                    {
                        Console.WriteLine("Invalid input");
                        return;
                    }
                    if (heuristic == 0)
                        Console.WriteLine("Code will run on [0]Manhattan  then   [1]Hamming  ");
                    else
                        Console.WriteLine("Code will run on **Only Manhattan** ");
                    for (int k = 0; k < 2; k++)  //O(1)   // To display the answer in Manhattan and hamming 
                    {
                        if (k == 0)
                            Console.WriteLine("[0]Manhattan     ");
                        if (heuristic == 1 && k == 1) break;
                        else if (k == 1)
                            Console.WriteLine("[1]Hamming       ");
                        Stopwatch stopwatch = new Stopwatch();
                        bool ReachedGoal = false;
                        // Begin timing , Start to solve 
                        stopwatch.Start();
                        System.Threading.Thread.Sleep(500);
                        GC.Collect();
                        Node temp = startnode.Astar(startnode, board, ref size, ref goal, k, ref ReachedGoal);//O(E Log V)
                        stopwatch.Stop();
                        if (!ReachedGoal) // If the Astar couldn't solve it 
                        {
                            Console.WriteLine("out of the while ");
                        }
                        else
                        {
                            Console.Write(" # of movements ");
                            Console.Write(temp.level);
                            End = temp;
                            Console.WriteLine();
                            Console.WriteLine(">>>Time : {0}", stopwatch.Elapsed);
                        }
                        Console.WriteLine("############################################################");
                    }
                }
                else if (ch == 1) // BFS Logic 
                {
                    BFSNode[,] graph = new BFSNode[size, size];
                    BFSNode firstnode = new BFSNode();
                    // Extract Frist Node 
                    for (int i = 0; i < size; i++)   // O(S square)
                    {
                        foreach (var el in Row[i])   // O (s)
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
                    // Start time 
                    bfswatch.Start();
                    System.Threading.Thread.Sleep(500);
                    BFSNode final = firstnode.BFS(firstnode, size, goal);   // O( E ) , Return Last Node (Result)
                    bfsend = final;
                    bfswatch.Stop();
                    if (final != null) // O(S) 
                    {
                        Console.WriteLine(" Found the goal ");
                        Console.WriteLine(">>> Time of solvability : {0}", bfswatch.Elapsed);
                        Console.WriteLine(" board ");
                        for (int i = 0; i < size; i++) // O(S)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                Console.Write(final.board[i, j]);
                            }
                            Console.WriteLine();
                        }
                    }

                    else     // If no Solution For the BFS 
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
                char c = Console.ReadLine().ToLower()[0];
                if (ch == 0)
                {
                    if (c.Equals('y'))
                    {
                        Console.WriteLine("What do you want ??   [0]Only Directions   OR   [1]Full Board");
                        int C = int.Parse(Console.ReadLine());
                        if (C != 0 && C != 1) Console.WriteLine("invalid input  ");
                        else Printpath(End, size, C);
                        Console.WriteLine();
                    }
                }
                else if (ch == 1)
                {
                    PrintpathBFS(bfsend, size);
                    Console.WriteLine();

                }
            }
            else if (ch == 1)  // For BFS Solution 
            {
                PrintpathBFS(bfsend, size); // Print path for solution , O(S)
                Console.WriteLine();

            }
            Console.WriteLine("Executed Successfully .... !!");
            Console.ReadKey();
        }

        // THIS FUNCTION Calculate the total steps, to solve N puzzle 
        static int InversionCount(int N, string[] puzzle)// O[(s)(s)] = O(S Squared)
        {
            int cnt = 0;
            for (int i = 0; i < N * N - 1; i++) // O(s) 
            {
                for (int j = i + 1; j < N * N; j++) // O(s) 
                {

                    int x = int.Parse(puzzle[i]);
                    int y = int.Parse(puzzle[j]);

                    if (x == 0 || y == 0)
                        continue;
                    if (x > y)
                        cnt++;

                }
            }
            return cnt;
        }

        // this function checks if N puzzles solvable or not according to Number of steps and blank space position 
        static bool CheckSolvability(int N, Dictionary<int, List<string>> puzzle, string[] temp_puzzle, int i) //O(N Squared)O[(s)(s)] = O(S Squared)

        {
            int cnt = InversionCount(N, temp_puzzle);// O[(s)(s)] = O(S Squared)
            int row_pos = i;//O(S)

            if (N % 2 == 0) // check if the N is even or not 
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


        // This Function for Printing our N puzzle Board Path in each move  
        public static bool PrintpathBFS(BFSNode end, int size)//O(N^3)
        {
            //base case of the recursion 
            if (end == null) // O(1)
                return false;
            PrintpathBFS(end.Parent, size); //T(N) = T(N-1) + THEATA(S) --> N*S --> N*N*N --> N^3 
            Console.WriteLine("Step # {0} ", step);
            step++;
            for (int i = 0; i < size; i++) // O(S) 
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

        public static bool Printpath(Node end, int size, int C)// Lower Bound: N , Upper bound: O(N^3)
        {
            //base case of the recursion 
            if (end == null) //O(1)
            {
                return false;
            }
            Printpath(end.Parent, size, C);// firstnode -> parent null 
            if (C == 0) // For printing only Directions 
            {
                if (end.level == 0)
                    end.direction = 4;
                //down 0 , up 1 ,right 2 , left 3
                switch (end.direction)
                {
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
            else if (C == 1) // For printing the whole path 
            {
                Console.WriteLine("Step # {0} ", end.level);
                for (int i = 0; i < size; i++) // O(S)
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
