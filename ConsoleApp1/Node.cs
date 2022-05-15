using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    class Node 
    {
        public Node Parent;
        public UInt16 G,F,H;
        public List<Node> Adjecants;
        public UInt16 X, Y;
        public UInt16 value;
        public int [,]board;
        public UInt16 level;
        public UInt16 direction;
        public Node(UInt16 x, UInt16 y, UInt16 value)
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

        public Node(Node parent, UInt16 g, UInt16 f, UInt16 h, List<Node> adjecants, UInt16 x, UInt16 y, UInt16 value)
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

        public void CalcH(int choice,int size,int [,]goal) //O(1)
        {
            int count = 0;
            switch (choice)
            {
                case 0: /// Manhatten distance calculation
                    int Col = 0;
                    int Row = 0;
                    if (this.Parent == null)
                    {
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
                        this.H = Convert.ToUInt16(count);
                        //Console.WriteLine("manhattan with N Square {0}", count);
                        break;
                    }
                    int man = this.Parent.H;
                    Col = (this.value - 1) % size;
                    Row = (this.value - 1) / size;
                    if (this.board[this.X, this.Y] == ((this.X * size + this.Y) + 1))
                    {
                        man++;
                    }
                    else
                    {
                        man -= Math.Abs(Row - this.X) + Math.Abs(Col - this.Y);
                        man += Math.Abs(Row - this.Parent.X) + Math.Abs(Col - this.Parent.Y);
                    }
                    //Console.WriteLine("manhattan with O(1) {0}", man);
                    this.H = Convert.ToUInt16(man);
                    break;

                case 1:/// Hamming distance calculation O(1)

                    //if (this.Parent == null)
                    //{
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (this.board[i, j] == 0) continue;
                                if (this.board[i, j] != goal[i, j]) count++;

                            }
                        }
                        this.H = Convert.ToUInt16(count);
                        break;
                    //}
                    //int ham = this.Parent.H;

                    //if (this.value == goal[this.X, this.Y])
                    //{
                    //    ham--;
                    //    Console.WriteLine("minus minus {0}", ham);

                    //}


                    //if (this.Parent.value != goal[this.Parent.X, this.Parent.Y])
                    //{
                    //    ham++;
                    //    Console.WriteLine("plus plus {0}", ham);
                    //}
                    //else
                    //{
                    //    ham--;
                    //}
                    
                    
                    //this.H = Convert.ToUInt16(ham);
                    //break;
            }
            
        }

        public void GetAdjecents(int size ,ref PriorityQ Astarlist,ref int heuristic,ref int [,]goal) //O(Log V)
        {

            for (UInt16 i = 0; i < 4; i++)
            {
                // check for availabilty of move 
                if (IsValid(this.X, this.Y, i, size))
                {
                    Tuple<int, int> index = Move(this.X, this.Y, i); //O(1)
                    if (!(this.Parent != null && this.Parent.X == index.Item1 && this.Parent.Y == index.Item2))
                    {
                        Node child = new Node
                        {
                            value = Convert.ToUInt16(this.board[index.Item1, index.Item2]),
                            Adjecants = new List<Node>(),
                            board = new int[size, size]
                        };
                        child.X = Convert.ToUInt16(index.Item1);
                        child.Y = Convert.ToUInt16(index.Item2);
                        child.Parent = this;
                        //copying parent board 
                        Array.Copy(this.board, child.board, size * size); //O(N Square)
                        child.Swap(ref child.board[this.X, this.Y], ref child.board[index.Item1, index.Item2]);
                        child.level = Convert.ToUInt16(this.level + 1);
                        child.direction = i;
                        this.Adjecants.Add(child); //O(1)
                    /*
                     *  condition to handle not to add the same node from path before
                     *  (x+1 , y) down , (x-1 , y) up ,
                     *  (x , y+1) right  , (x ,y-1) left
                     *   down 0 , up 1 ,right 2 , left 3
                     */
                    }
                    else if (this.Parent == null)
                    {
                        if (!(this.Parent.X == index.Item1 && this.Parent.Y == index.Item2))
                        {
                            Node child = new Node
                            {
                                value = Convert.ToUInt16(this.board[index.Item1, index.Item2]),
                                Adjecants = new List<Node>(),
                                board = new int[size, size]
                            };
                            child.X = Convert.ToUInt16(index.Item1);
                            child.Y = Convert.ToUInt16(index.Item2);
                            child.Parent = this;
                            //copying parent board 
                            Array.Copy(this.board, child.board, size * size); //O(N Square)
                            child.Swap(ref child.board[this.X, this.Y], ref child.board[index.Item1, index.Item2]);
                            child.level = Convert.ToUInt16(this.level + 1);
                            child.direction = i;
                            this.Adjecants.Add(child); //O(1)
                        }
                    }
                }
                
            }

            foreach (var neighbour in this.Adjecants)
            {
                neighbour.G = Convert.ToUInt16(this.G + 1);//O(1)
                // 0 Manhatten distance , 1 Hamming distance 
                neighbour.CalcH(heuristic, size,goal);//O(1)
                neighbour.F = Convert.ToUInt16(neighbour.G + neighbour.H);
                neighbour.Parent = this;
                Astarlist.Enqueue(neighbour);//O(Log V)
            }
        }

        public Node Astar(Node startnode,int [,]board,ref int size,ref int[,]goal,int heuristic ,ref bool ReachedGoal)//O(E Log V)
        {
            PriorityQ Astarlist = new PriorityQ();
            Console.WriteLine("Start node at x,y " + startnode.X + " " + startnode.Y);
            startnode.board = board;
            startnode.G = 0;
            startnode.CalcH(heuristic, size, goal); //O(1)
            startnode.F = Convert.ToUInt16(startnode.G + startnode.H);
            startnode.level = 0;
            startnode.Parent = null;
            Astarlist.Enqueue(startnode);//O(Log V)
            Node temp;
            while (!Astarlist.Empty()) // iterations (max E)  * Complexity body (Log V)
            {
                temp = Astarlist.Dequeue();//O(Log V)
                //if the heuristic value to the peek node is 0 then we reached our goal 
                if (temp.H == 0)
                {
                    ReachedGoal = true;
                    Console.WriteLine("Found the goal ");
                    return temp;
                }
                //calculate each neighbour and add it to priorityqueue 
                temp.GetAdjecents(size, ref Astarlist, ref heuristic, ref goal);//O(Log V)
                temp = null; 
            }
            return null;
        }

        private static Tuple<int, int> Move(int x, int y, int i) //O(1)
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

        public void Swap(ref int x, ref int y) //O(1)
        {
            int t = x;
            x = y;
            y = t;
        }
        
    }
}
