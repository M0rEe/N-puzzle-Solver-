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

        public void GetAdjecents(int size)//O(S)
        {
            for (int i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))//O(1)
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i);//O(1)
                    BFSNode child = new BFSNode(size, board[index.Item1, index.Item2], this.board);//O(1)
                    //copying parent board 
                    Array.Copy(this.board, child.board, size * size);//O(S)
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
        //BFS Logic
        public BFSNode BFS(BFSNode start,int size ,int [,] goalboard)  // O( E V ) 
        {
            List<BFSNode> openlist = new List<BFSNode>();
            List<BFSNode> closedlist = new List<BFSNode>();
            openlist.Add(start); // O(1)
            while(openlist.Count > 0) //# iterations E * Complexity of body O(V) = //O(E*V) 
            {
                BFSNode current = openlist[0];
                openlist.RemoveAt(0);  // O(1)
                closedlist.Add(current); // O(1)
                current.GetAdjecents(size);//O(S)
                foreach (var child in current.Adjecents)//O(1) --> total O(v)
                {
                    if (Checkboard(child.board, goalboard, size))//O(S)
                    {
                        count++;
                        Console.WriteLine("Found Goal ");
                        Console.WriteLine("out from child  = {0} ", count);
                        Console.WriteLine("Closed list count  = {0} ", closedlist.Count);
                        Console.WriteLine("Open list count  = {0} ", openlist.Count);
                        return child;
                    }
                    if (!openlist.Contains(child) && !closedlist.Contains(child)) // O(v)
                    {
                        child.Parent = current;
                        openlist.Add(child);//O(1)
                        count++;
                    }
                }

            }
            return null;
        }
        // check if the board is alreasy solved or not yet  
        bool Checkboard(int[,]first,int [,]second,int size)//O(S)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (first[i, j] == second[i, j]) 
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }

        // creates the move itself 
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

        // Check the next move within board bounders or not 
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
