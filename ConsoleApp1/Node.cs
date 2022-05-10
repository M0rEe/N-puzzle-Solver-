using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    class Node 
    {
        public Node Parent;
        public int G,F,H;
        public List<Node> Adjecants;
        public int X, Y;
        public int value;
        public int [,]board;
        public int level;
        public int direction;
        public Node(int x, int y, int value)
        {
            this.value = value;
            this.X = x;
            this.Y = y;
            this.G = 1;
            this.H = 0;
            this.F = 0;
            this.Parent = null;
            this.Adjecants = new List<Node>();
        }
        public Node()
        {
        }

        public Node(Node parent, int g, int f, int h, List<Node> adjecants, int x, int y, int value)
        {
            Parent = parent;
            G = g;
            F = f;
            H = h;
            Adjecants = adjecants;
            X = x;
            Y = y;
            this.value = value;
        }

        public void CalcH(int choice,int size,int [,]goal)
        {
            int count = 0;
            switch (choice)
            {
                case 0: /// Manhatten distance calculation
                    int Col = 0;
                    int Row = 0;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (this.board[i, j] == 0) continue;
                            else if (this.board[i, j] != ((i * size + j) + 1))
                            {
                                Col = ((this.board[i, j] - 1) % size);
                                Row = (this.board[i, j] - 1) / size;
                                count += Math.Abs(Row - i) + Math.Abs(Col - j);
                            }
                        }
                    }
                    this.H = count;
                    break;

                case 1:/// Hamming distance calculation
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (this.board[i, j] == 0) continue;
                            if (this.board[i, j] != goal[i,j]) count++;
                            
                        }
                    }

                    this.H = count;
                    break;
            }
            
        }
        public void GetAdjecents(int size ,PriorityQ Astarlist,int heuristic,int [,]goal  )
        {

            for (int i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i);
                    Node child = new Node
                    {
                        value = this.board[index.Item1, index.Item2],
                        Adjecants = new List<Node>(),
                        board = new int[size, size]
                    };
                    //copying parent board 
                    Array.Copy(this.board, child.board, size * size);
                    child.Swap(ref child.board[this.X, this.Y], ref child.board[index.Item1, index.Item2]);
                    child.X = index.Item1;
                    child.Y = index.Item2;
                    child.level = this.level + 1;
                    child.Parent = this;
                    /*
                     *  condition to handle not to add the same node from path before
                     *  (x+1 , y) down , (x-1 , y) up ,
                     *  (x , y+1) right  , (x ,y-1) left
                     *   down 0 , up 1 ,right 2 , left 3
                     */
                    if (!(this.Parent != null && this.Parent.X == child.X && this.Parent.Y == child.Y))
                    {
                        child.direction = i;
                        this.Adjecants.Add(child);
                    }
                }
                else
                {
                    continue;
                }
            }

            foreach (var neighbour in this.Adjecants)
            {
                neighbour.G = this.G + 1;
                // 0 Manhatten distance , 1 Hamming distance 
                neighbour.CalcH(heuristic, size,goal);
                neighbour.F = neighbour.G + neighbour.H;
                neighbour.Parent = this;
                Astarlist.enqueue(neighbour);
            }
        }
        public Node Astar(Node startnode,int [,]board,int size,int[,]goal,int heuristic ,ref bool ReachedGoal)
        {
            PriorityQ Astarlist = new PriorityQ();
            Console.WriteLine("Start node at x,y " + startnode.X + " " + startnode.Y);
            startnode.board = board;
            startnode.G = 0;
            startnode.CalcH(heuristic, size, goal);
            startnode.F = startnode.G + startnode.H;
            startnode.level = 0;
            startnode.Parent = null;
            Astarlist.enqueue(startnode);
            Node temp = new Node();
            while (!Astarlist.empty())
            {
                temp = Astarlist.dequeue();
                //if the heuristic value to the peek node is 0 then we reached our goal 
                if (temp.H == 0)
                {
                    ReachedGoal = true;
                    Console.WriteLine("Found the goal ");
                    return temp;
                }
                temp.GetAdjecents(size, Astarlist, heuristic, goal);

                temp.Adjecants = null;
                temp = null;

            }
            return null;
        }
        private static Tuple<int, int> Move(int x, int y, int i)
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

        private static bool IsValid(int x, int y, int i, int size)
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

        public void Swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }
        
    }
}
