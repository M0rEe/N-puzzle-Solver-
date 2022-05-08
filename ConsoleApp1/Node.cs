using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Node : IComparable<Node>
    {
        public Node Parent;
        public int G,F,H;
        public List<Node> Adjecants;
        public int X, Y;
        public int value;
        public int [,]board;
        public int[,] goal;
        public int level;
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public void swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }
        
        public int CompareTo(Node other)
        {
            if (other.H < this.H){
                return 1;
            }else{
                return 0 ;
            }
        }
    }
}
