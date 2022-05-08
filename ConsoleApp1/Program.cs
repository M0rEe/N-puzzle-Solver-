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
            Node[,] element = new Node [size,size];
            Node startnode = new Node(9, 9, 9);
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
            Stopwatch valid = new Stopwatch();
            valid.Start();
            System.Threading.Thread.Sleep(500);
            bool x = CheckSolvability(size, Row, temp_arr);
            valid.Stop();
            if (x)
            {
                Console.Write(">>>");
                Console.WriteLine("Solvable");

                Stopwatch stopwatch = new Stopwatch();
                // Begin timing
                stopwatch.Start();

                System.Threading.Thread.Sleep(500);
                PriorityQ Astarlist = new PriorityQ();
                Console.WriteLine("Start node at x,y " + startnode.X + " " + startnode.Y);
                startnode.board = board;
                startnode.G = 0;
                startnode.CalcH(1, size);
                startnode.CalcF();
                startnode.level = 0;
                startnode.Parent = null;
                Astarlist.enqueue(startnode);
                Node temp = new Node();
                bool ReachedGoal = false;
                while (!Astarlist.empty())
                {
                    temp = Astarlist.dequeue();
                    if (temp.H == 0)
                    { //if the heuristic value to the peek node is 0 then we reached our goal 
                        ReachedGoal = true;
                        Console.WriteLine("Found the goal ");
                        break;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        // check for availabilty of move 
                        if (isValid(temp.X, temp.Y, i, size))
                        {
                            Tuple<int, int> index = Move(temp.X, temp.Y, i);
                            Node child = new Node();
                            child.value = temp.board[index.Item1, index.Item2];
                            child.Adjecants = new List<Node>();
                            child.board = new int[size, size];
                            //copying parent board 
                            Array.Copy(temp.board, child.board, size * size);
                            child.swap(ref child.board[temp.X, temp.Y], ref child.board[index.Item1, index.Item2]);
                            child.X = index.Item1;
                            child.Y = index.Item2;
                            child.level = temp.level + 1;
                            child.Parent = temp;
                            //condition to handle not to add the same node from path before
                            if (!(temp.Parent != null && temp.Parent.X == child.X && temp.Parent.Y == child.Y))
                            {
                                temp.Adjecants.Add(child);
                            }
                            /*  
                             *  (x+1 , y) down , (x-1 , y) up ,
                             *  (x , y+1) right  , (x ,y-1) left
                             *   down 0 , up 1 ,right 2 , left 3
                             */
                        }
                        else
                        {
                            continue;
                        }
                    }

                    foreach (var neighbour in temp.Adjecants)
                    {
                        neighbour.G = temp.G + 1;
                        // 0 Manhatten distance , 1 Hamming distance 
                        neighbour.CalcH(0, size);
                        neighbour.CalcF();
                        neighbour.Parent = temp;
                        Astarlist.enqueue(neighbour);
                    }
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
                    Console.WriteLine();
                    Console.WriteLine("Time : {0}", stopwatch.Elapsed);
                }
            }
            else
            {
                Console.Write(">>>");
                Console.WriteLine("NOT Solvable");
                Console.WriteLine("Time : {0}", valid.Elapsed);
            }
            Console.WriteLine();
            
        }

        private static Tuple<int , int> Move(int x, int y,int i)
        {
            switch (i)
            {
                case 0:
                    return Tuple.Create(x + 1, y);
                case 1:
                    return Tuple.Create(x - 1, y);
                case 2:
                    return Tuple.Create(x, y + 1);
                case 3:
                    return Tuple.Create(x, y - 1);
                default:
                    return Tuple.Create(x, y);
            }
        }

        private static bool isValid(int x, int y, int i,int size )
        {
            switch (i)
            {
                case 0:
                    if (x + 1 >= size)
                        return false;
                    else
                        return true;
                case 1:
                    if (x - 1 < 0)
                        return false;
                    else
                        return true;
                case 2:
                     if (y + 1 >= size)
                        return false;
                    else
                        return true;
                case 3:
                    if (y - 1 < 0)
                        return false;
                    else
                        return true;
                default:
                    return false;
            }
        }


        static int inversionCount(int N, string[] puzzle)
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
            int cnt = inversionCount(N, temp_puzzle);
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

    }
}
