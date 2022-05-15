using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Color zerocolor = Color.Violet;
        Color temp = Color.White;
        int size = 0;
        List<Label> grid = new List<Label>();
        int index = 0;
        public static string route = "";
        private void Button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();
            string path = OpenFile.FileName;

            FileStream file;
            StreamReader sr;
            string line;
            TextReader origConsole = Console.In;
            file = new FileStream(path, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(file);
            line = sr.ReadLine();
            size = int.Parse(line);
            int[,] goal = new int[size, size];
            line = sr.ReadLine();
            Dictionary<int, List<string>> Row = new Dictionary<int, List<string>>();
            int indexi = 0, indexj = 0;
            for (int i = 0; i < size;)
            {
                if (line == "")
                {
                    line = sr.ReadLine();
                    i = 0;
                    continue;
                }
                Row.Add(i, new List<string>());
                List<string> vertices = line.Split(' ').ToList();
                Row[i] = vertices;
                for (int j = 0; j < size; j++)
                {
                    goal[i, j] = i * size + (j + 1);
                    indexj = j;
                }
                indexi = i;
                i++;
                line = sr.ReadLine();
            }

            goal[indexi, indexj] = 0;
            sr.Close();
            file.Close();
            ////////////////////////////////////////////////
            listBox1.Items.Clear();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Label lbl = new Label
                    {
                        Text = Row[i][j]
                    };
                    if (lbl.Text.Equals("0")) index = grid.Count;
                    grid.Add(lbl);
                    lbl.BorderStyle = BorderStyle.FixedSingle;
                    lbl.SetBounds((j * panel1.Size.Width / size), (i * panel1.Size.Height / size),
                        panel1.Size.Width / size, panel1.Size.Height / size);
                    lbl.Font = new Font("Calibri", 22);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    panel1.Controls.Add(lbl);

                }
            }
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Value = 5;

            //////////////////////////////////////////////////
            int cnt = 0;
            string[] temp_arr = new string[size * size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp_arr[cnt] = Row[i][j];
                    cnt++;
                }
            }

            int[,] board = new int[size, size];
            Node[,] element = new Node[size, size];
            Node startnode = new Node();
            Node End = new Node();
            for (int i = 0; i < size; i++)
            {
                foreach (var el in Row[i])
                {
                    int j = Row[i].IndexOf(el);
                    element[i, j] = new Node(Convert.ToUInt16(i), Convert.ToUInt16(j), UInt16.Parse(el));
                    board[i, j] = int.Parse(el);
                    if (el.Equals("0"))
                    {
                        startnode = element[i, j];
                    }
                }
            }
            Stopwatch b = new Stopwatch();
            b.Start();
            bool Sol = CheckSolvability(size, Row, temp_arr);
            b.Stop();
            if (Sol)
            {
                label4.Text = "Time Of Solavability : \n" + b.Elapsed.ToString(); 
                bool ReachedGoal = false;
                // Begin timing
                GC.Collect();
                Stopwatch a = new Stopwatch();
                a.Start();
                Node temp = startnode.Astar(startnode, board, ref size, goal, 0, ref ReachedGoal);//O(E Log V)
                a.Stop();
                if (!ReachedGoal)
                {
                    MessageBox.Show("OUT OF WHILE LOOP!!!");
                }
                else
                {
                    label1.Text = "Number Of Moves : \n" + temp.level.ToString();
                    label3.Text = "TIme To Solve: \n" + a.Elapsed.ToString();
                    End = temp;
                }
                
            }
            else
            {
                MessageBox.Show("Not solvable !!");
            }
            if (Sol)
            {
                bool ret = Printpath(End, size);
                foreach (var c in route)
                {
                    listBox1.Items.Add(c);
                }
                listBox1.SelectedIndex = 0;
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Maximum = Convert.ToInt32(End.level);
                progressBar1.Value = 0;

                timer1.Enabled = true;
                timer1.Start();
                progressBar1.Value++;
                label2.Text = $"{progressBar1.Value}/{route.Length}";
            }
        }
        // O[(s)(s)] = O(S Squared)
        static int InversionCount(int N, string[] puzzle)
        {
            int cnt = 0;
            for (int i = 0; i < N * N - 1; i++)
            {
                for (int j = i + 1; j < N * N; j++)
                {

                    int x = int.Parse(puzzle[i]);
                    int y = int.Parse(puzzle[j]);

                    if (x == 0 || y == 0)
                        continue;
                    if (x > y)
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }
        //O(N Squared)
        static int Blankposition(Dictionary<int, List<string>> puzzle)
        {
            //string blank = "0";

            for (int i = puzzle.Count - 1; i >= 0; i--)
            {
                for (int j = puzzle.Count - 1; j >= 0; j--)
                {
                    if (puzzle[i][j].Equals("0"))
                    {
                        return puzzle.Count - i;
                    }
                }
            }
            return 0;
        }
        //O(N Squared)O[(s)(s)] = O(S Squared)
        static bool CheckSolvability(int N, Dictionary<int, List<string>> puzzle, string[] temp_puzzle)
        {
            int cnt = InversionCount(N, temp_puzzle);// O[(s)(s)] = O(S Squared)
            int row_pos = Blankposition(puzzle);//O(N Squared)

            if (N % 2 == 0)
            {
                if ((row_pos % 2 == 0 && cnt % 2 != 0))
                    return true;
                if ((row_pos % 2 != 0 && cnt % 2 == 0))
                    return true;
            }
            else
            {
                if (cnt % 2 == 0)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        public static bool Printpath(Node end, int size)//O(S)
        {
            //base case of the recursion 
            if (end == null) //O(1)
            {
                return false;
            }
            Printpath(end.Parent, size);// firstnode -> parent null 

            if (end.level == 0) end.direction = 4;
            //down 0 , up 1 ,right 2 , left 3
            switch (end.direction)
            {
                case 0:
                    route += "D";
                    break;
                case 1:
                    route += "U";
                    break;
                case 2:
                    route += "R";
                    break;
                case 3:
                    route += "L";
                    break;
                default:
                    break;
            }
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            char currentmove = listBox1.SelectedItem.ToString()[0] ;
            Label zero =  grid.ElementAt(index);
            Label m;
            if (timer1.Enabled && progressBar1.Value < progressBar1.Maximum )
            {
                progressBar1.Value++;
                label2.Text = $"{progressBar1.Value}/{route.Length}";
            }
            switch (currentmove)
            {
                case 'U':
                    m = grid.ElementAt(index - size);
                    zero.Text = m.Text;
                    zero.BackColor = temp;
                    m.Text = "0";
                    m.BackColor = zerocolor;
                    index -= size;
                    break;
                case 'D':
                    m = grid.ElementAt(index + size);
                    zero.Text = m.Text;
                    zero.BackColor = temp;
                    m.Text = "0";
                    m.BackColor = zerocolor;

                    index += size;
                    break;
                case 'L':
                    m = grid.ElementAt(index - 1);
                    zero.Text = m.Text;
                    zero.BackColor = temp;
                    m.Text = "0";
                    m.BackColor = zerocolor;

                    index -= 1;
                    break;
                case 'R':
                    m = grid.ElementAt(index + 1);
                    zero.Text = m.Text;
                    zero.BackColor = temp;
                    m.Text = "0";
                    m.BackColor = zerocolor;

                    index += 1;
                    break;
            }
            if (listBox1.SelectedIndex + 1 == route.Length)
            {
                timer1.Enabled = false;
                timer1.Stop();
                route = "";
            }
            else
                listBox1.SelectedIndex++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            label2.BackColor = System.Drawing.Color.Transparent; ;
        }
    }
}

