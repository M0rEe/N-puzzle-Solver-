using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    class PriorityQ 
    {

        public Node[] Arr;
        int length = 0;

        public void Enqueue(Node x)//O(Log V)
        {
            length = length + 1;
            Arr[length] = null;  //assuming all the numbers greater than 0 are to be inserted in queue.
            Arr[length] = x;
            int i = length;
            while (i > 1 && Arr[i / 2].F >= Arr[i].F)//O(Log V)
            {
                Swap(ref Arr[i / 2], ref Arr[i]);
                i = i / 2;//1
            }
        }
        public Node Dequeue()//O(Log V)
        {
            if (length == 0)
            {
                throw new InvalidOperationException("queue is Empty");
            }
            Node min = Arr[1];
            Arr[1] = Arr[length];//1
            length = length - 1;
            Min_heap(1, length);
            return min;
        }

        public bool Empty()
        {
            return (length == 0);
        }

        public PriorityQ()
        {
            Arr = new Node[10000000];
        }
        public int Count() { return length; }

       
        void Min_heap(int i, int N)//O(Log V)
        {
            // to get index of left child of Node at index i 
            int left = 2 * i;
            // to get index of right child of Node at index i
            int right = 2 * i + 1;
            int smallest;

            if (left <= N && Arr[left].F < Arr[i].F)
                smallest = left;
            else
                smallest = i;
            if (right <= N && Arr[right].F < Arr[smallest].F)//1
                smallest = right;
            if (smallest != i)//1
            {
                Swap(ref Arr[i], ref Arr[smallest]);//1
                Min_heap(smallest, N);
            }
        }
        
        void Swap(ref Node x, ref Node y)//1
        {
            Node t = x;
            x = y;
            y = t;
        }

    }
}
