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

        public void CalcH(int choice, Node goal)
        {
            int i = 0, count = 0;
            switch (choice)
            {
                case 0: /// Manhatten distance calculation
                    this.H = Math.Abs(this.X-goal.X) + Math.Abs(this.Y - goal.Y);
                    break;

                case 1:/// Hamming distance calculation
                    while(i < goal.value.ToString().Length)
                    {
                        if (this.value.ToString()[i] != goal.value.ToString()[i])
                            count++;
                        i++;
                    }
                    this.H = count;
                    break;
            }
            
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public int CompareTo(Node other)
        {
            if (other.F > this.F){
                return 1;
            }else{
                return 0 ;
            }
        }
    }
}
