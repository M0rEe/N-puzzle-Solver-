using System;
using System.Collections.Generic;
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
            //better way can use priority queue insted of lists 
            PriorityQ Astarlist = new PriorityQ();
            Console.WriteLine("Start node at x,y "+startnode.X + " " + startnode.Y );
            startnode.board = board;
            startnode.G = 0;
            startnode.CalcH(1,size);
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
                    if (isValid(temp.X, temp.Y, i,size)) 
                    {
                        Tuple<int ,int> index = Move(temp.X, temp.Y,i);
                        Node child = new Node();
                        child.value = temp.board[index.Item1, index.Item2];
                        child.Adjecants = new List<Node>();
                        child.board = new int[size, size];
                        //copying parent board 
                        Array.Copy(temp.board,child.board,size*size);
                        child.swap(ref child.board[temp.X,temp.Y], ref child.board[index.Item1, index.Item2]);
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

                foreach(var neighbour in temp.Adjecants)
                {
                    neighbour.G = temp.G + 1;
                    // 0 Manhatten distance , 1 Hamming distance 
                    neighbour.CalcH(0,size); 
                    neighbour.CalcF();
                    neighbour.Parent = temp;
                    Astarlist.enqueue(neighbour);
                }
            }

            if (!ReachedGoal)
            {
               Console.WriteLine("out of the while ");
            }
            else
            {
                Console.Write("# of movements ");
                Console.Write(temp.level);
                Console.WriteLine();
            }


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
    }
}
