using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class BFSNode
    {
        public int X,Y;
        public int[,] board;
        public int Value;
        public List<BFSNode> Adjecents;
        public BFSNode Parent;

        public BFSNode(int size,int val,int [,] brd )
        {
            this.Adjecents = new List<BFSNode>();
            this.board = new int[size,size];
            Array.Copy(brd,this.board,size*size);
            this.Value = val;
        }
        public BFSNode()
        {}
        public BFSNode(BFSNode p)
        {
            this.Parent = p;
        }

        public void GetAdjecents(int size)
        {
            for (int i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i);
                    BFSNode child = new BFSNode(size, board[index.Item1, index.Item2], this.board);
                    //copying parent board 
                    Array.Copy(this.board, child.board, size * size);
                    child.Swap(ref child.board[this.X, this.Y], ref child.board[index.Item1, index.Item2]);
                    child.X = index.Item1;
                    child.Y = index.Item2;
                    child.Parent = this;
                    if (!(this.Parent != null && this.Parent.X == child.X && this.Parent.Y == child.Y))
                    {
                        this.Adjecents.Add(child);
                    }
                }
            }
        }
        static int count = 0;
        public BFSNode BFS(BFSNode start,int size ,int [,] goalboard)
        {
            List<BFSNode> openlist = new List<BFSNode>();
            List<BFSNode> closedlist = new List<BFSNode>();
            openlist.Add(start);
            while(openlist.Count > 0 )
            {
                BFSNode current = openlist[0];
                current.GetAdjecents(size);
                foreach (var child in current.Adjecents)
                {
                    if (Checkboard(child.board, goalboard, size))
                    {
                        count++;
                        Console.WriteLine("Found Goal ");
                        Console.WriteLine("out from child  = {0} ", count);
                        Console.WriteLine("Closed list count  = {0} ", closedlist.Count);
                        Console.WriteLine("Open list count  = {0} ", openlist.Count);
                        return child;
                    }
                    if (!openlist.Contains(child) && !closedlist.Contains(child)) 
                    {
                        child.Parent = current;
                        openlist.Add(child);
                        count++;
                    }
                }
                openlist.RemoveAt(0);
                closedlist.Add(current);

            }
            return null;
        }
        bool Checkboard(int[,]first,int [,]second,int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (first[i, j] == second[i, j]) continue;
                    else return false;
                }
            }
            return true;
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
