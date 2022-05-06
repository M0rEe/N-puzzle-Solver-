using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            file = new FileStream("8 Puzzle (1).txt", FileMode.Open, FileAccess.Read);

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
            /*
            for (int i =0;i<size;i++)
            {
                for (int j =0; j< size;j++)
                {
                    Console.Write(goal[i, j] );
                }
                Console.WriteLine();
            }
            */
            sr.Close();
            file.Close();
            int[,] board  =new int[size,size]; 
            Node[,] element = new Node [size,size];
            Node startnode = new Node(9, 9, 9), child;
            for (int i = 0; i < size; i++)
            {
                foreach (var el in Row[i])
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(i,j,int.Parse(el));
                    board[i, j] = int.Parse(el);
                    element[i, j].goal = goal;
                    //Console.Write(el + " " + element.X + " " + element.Y + " \\ "+ element.value);
                    if (el.Equals("0"))
                    {
                        startnode = element[i,j];
                        
                    }
                }
            }
            //better way can use priority queue insted of lists 
            PriorityQ<Node> Astarlist = new PriorityQ<Node>();
            //List<Node> Openlst = new List<Node>();
            List<Node> Closedlst = new List<Node>();
            Console.WriteLine("Start node at x,y "+startnode.X + " " + startnode.Y );
            startnode.board = board;
            startnode.G = 0;
            startnode.CalcH(0,size);
            startnode.CalcF();
            Astarlist.Enqueue(startnode);
            Node temp;
            
            while (Astarlist.Count() > 0)
            {
                // if in manhattan then each node should be 0 from its index 
                //if in hamming then the whole board should be 0 
                temp = Astarlist.Peek();
                if (temp.H == 0)  //if the heuristic value to the peek node is 0 then we reached our goal 
                    break;
       
                for (int i = 0; i < 4; i++)
                {
                    if(isValid(temp.X, temp.Y, i,size)) // check for availabilty of move 
                    {
                        Tuple<int ,int> index = Move(temp.X, temp.Y,i);
                        child = element[index.Item1, index.Item2];
                        child.board = new int[size, size];
                        Array.Copy(temp.board,child.board,size*size);
                        Console.WriteLine(child.board[temp.X, temp.Y]);
                        child.swap(ref child.board[temp.X,temp.Y], ref child.board[index.Item1, index.Item2]);
                        child.X = index.Item1;
                        child.Y = index.Item2;
                        Console.WriteLine("Changed to ");
                        Console.WriteLine(child.board[temp.X, temp.Y]);
                        temp.Adjecants.Add(child);
                        /*  
                         *  (x+1 , y) down , (x-1 , y) up ,
                         *  (x , y+1) right  , (x ,y-1) left
                         *   down 0 , up 1 ,right 2 , left 3
                         */
                        
                    }
                }

                foreach(var neighbour in temp.Adjecants)
                {
                    /*
                    if (neighbour.value == 0) //TODO: Goal condition we didnt detect our goal yet 
                    {
                        Reached_goal = true;
                        break;
                    }
                    */
                    neighbour.G = temp.G + 1;//TODO:: what is our Goal? 
                    neighbour.CalcH(0,size); // 0 manhatten distance , 1 hamming distance 
                    neighbour.CalcF();
                    neighbour.Parent = temp;
                    Astarlist.Enqueue(neighbour);
                    Console.WriteLine("Neighbour data "+neighbour.value);
                    Console.WriteLine("Manhattan distance ");
                    Console.WriteLine(neighbour.H);
                }
                Astarlist.Dequeue();
                Closedlst.Add(temp);
                temp = Astarlist.Peek();
                Console.WriteLine("astar peak "+temp.value + "  " + temp.Parent.value);
                Console.WriteLine(Astarlist.Count());
                Console.ReadLine();
            }
            Console.WriteLine("out");
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
