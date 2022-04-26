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
            Dictionary<int, List<string>> Row = new Dictionary<int, List<string>>();
            for (int i = 0; i < size; i++)
            {
                line = sr.ReadLine();
                Row.Add(i, new List<string>());
                List<string> vertices = line.Split(' ').ToList();
                Row[i] = vertices;
            }
            sr.Close();
            file.Close();

            Node[,] element = new Node [size,size]; 
            Node startnode =new Node(9,9,9),child;
            for (int i = 0; i < size; i++)
            {
                foreach (var el in Row[i])
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(i,j,int.Parse(el));
                    //Console.Write(el + " " + element.X + " " + element.Y + " \\ "+ element.value);
                    if (el.Equals("0"))
                        startnode = element[i,j];
                }
                Console.WriteLine();
            }
            List<Node> Openlst = new List<Node>();
            List<Node> Closedlst = new List<Node>();
            Openlst.Add(startnode);
            //Console.Write(startnode.X + " " + startnode.Y + " \\ ");
            startnode.F = 0;
            Node temp;
            while (Openlst.Count > 0)
            {
                Openlst.Sort();
                temp = Openlst.ElementAt(0);
                for (int i = 0; i < 4; i++)
                {
                    if(isValid(temp.X, temp.Y, i,size)) // check for availabilty of move 
                    {
                        Tuple<int ,int> index = Move(temp.X, temp.Y,i);
                        child = element[index.Item1, index.Item2];
                        /*  
                         *  (x+1 , y) down , (x-1 , y) up ,
                         *  (x , y+1) right  , (x ,y-1) left
                         *   down 0 , up 1 ,right 2 , left 3
                         */
                        temp.Adjecants.Add(child);
                        /*
                        Console.WriteLine("Moved " + i);
                        Console.WriteLine(child.X );
                        Console.WriteLine(child.Y );
                        Console.WriteLine(temp.Adjecants.Count);
                        Console.WriteLine("done with case" + i);
                        */
                    }
                }
                Openlst.RemoveAt(0);
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
                    if (x + 1 > size)
                        return false;
                    else
                        return true;
                case 1:
                    if (x - 1 < 0)
                        return false;
                    else
                        return true;
                case 2:
                     if (y + 1 > size)
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
