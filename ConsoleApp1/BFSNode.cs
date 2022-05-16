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

        public void GetAdjecents(int size)//O(N²)
        {
            for (int i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))//O(1)
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i);//O(1)
                    BFSNode child = new BFSNode(size, board[index.Item1, index.Item2], this.board);//O(1)
                    //copying parent board 
                    Array.Copy(this.board, child.board, size * size);//O(N²)
                    child.Swap(ref child.board[this.X, this.Y], ref child.board[index.Item1, index.Item2]);
                    child.X = index.Item1;
                    child.Y = index.Item2;
                    child.Parent = this;
                    if (!(this.Parent != null && this.Parent.X == child.X && this.Parent.Y == child.Y))
                    {
                        this.Adjecents.Add(child);//O(1)
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
            while(openlist.Count > 0)//# iterations E * Complexity of body O(N²) = //O(E*N²) E>N² ?? 
            {
                BFSNode current = openlist[0];
                current.GetAdjecents(size);//O(N²)
                foreach (var child in current.Adjecents)//O(1)
                {
                    if (Checkboard(child.board, goalboard, size))//O(N²)
                    {
                        count++;
                        Console.WriteLine("Found Goal ");

                        Console.WriteLine("out from child  = {0} ", count);
                        Console.WriteLine("Closed list count  = {0} ", closedlist.Count);
                        Console.WriteLine("Open list count  = {0} ", openlist.Count);
                        return child;
                    }
                    if (!openlist.Contains(child) && !closedlist.Contains(child)) //O(N)
                    {
                        child.Parent = current;
                        openlist.Add(child);//O(1)
                        count++;
                    }
                }
                openlist.RemoveAt(0);
                closedlist.Add(current);

            }
            return null;
        }
        bool Checkboard(int[,]first,int [,]second,int size)//O(N²)
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
        private static Tuple<int, int> Move(int x, int y, int i)//O(1)
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

        private static bool IsValid(int x, int y, int i, int size)//O(1)
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

        public void Swap(ref int x, ref int y)//O(1)
        {
            int t = x;
            x = y;
            y = t;
        }

    }
}
