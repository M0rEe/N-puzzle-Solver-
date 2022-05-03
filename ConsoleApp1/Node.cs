using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Node : IComparer<Node>
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
            this.G = 999999999;
            this.H = 999999999;
            this.F = 999999999;
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

        public Node Compare(Node x, Node y)
        {
            if (x.F < y.F)
            {
                return x;
            }
            return y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 1006953623;
            hashCode = hashCode * -1521134295 + EqualityComparer<Node>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + F.GetHashCode();
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Node>>.Default.GetHashCode(Adjecants);
            return hashCode;
        }

        int IComparer<Node>.Compare(Node x, Node y)
        {
            return ((IComparer<Node>)Parent).Compare(x, y);
        }
    }
}
