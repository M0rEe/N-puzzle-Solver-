using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    enum Dir { down  , up  , right  , left };
    class Node 
    {
        public Node Parent;
        public int G,F,H;
        public List<Node> Adjecants;
        public int X, Y;
        public int value;
        public int [,]board;
        public int[,] goal;
        public int level;
        public int direction;
        public Node(int x,int y,int value)
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

        public Node(Node parent, int g, int f, int h, List<Node> adjecants, int x, int y,int value)
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

        public void CalcF()
        {
            this.F = this.G + this.H;
        }

        public void CalcH(int choice,int size)
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
                            if (this.board[i, j] != this.goal[i,j]) count++;
                            
                        }
                    }

                    this.H = count;
                    break;
            }
            
        }
        public void GetAdjecents(int size ,PriorityQ Astarlist,int heuristic )
        {

            for (int i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i);
                    Node child = new Node
                    {
                        goal = goal,
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
                neighbour.CalcH(heuristic, size);
                neighbour.CalcF();
                neighbour.Parent = this;
                Astarlist.enqueue(neighbour);
            }
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
