using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        int no3=0, no6=0, no4=0, no2=0, no5=0;
        int filled, depth;
        int[,] arr = new int[10, 10];
        string[,] str = new string[10, 10];
        int[,,] block = new int[10, 10, 2];
        bool change, error;
        string text, dp;

        public Form1()
        {
            dp = "";
            filled = 0;
            depth = 0;
            error = false;
            InitializeComponent();
            populate();
        }

        void trySolve()
        {
            dp = dp + depth.ToString()+" ";
            do
            {
                change = false;
                soleCandidate(1);
                UniqueCandidate();
                CorrectRowsColsViaBlocks();
                NakedSubset();
                CorrectBlocksViaRowsCols();
                FindHook();
                HiddenSubset();
            } while (change && filled != 81);

            if (filled == 81)
            {
                MessageBox.Show(
                "NakedSubset() succeeded " + no4.ToString() + " times\n"
                + "FindHook() succeeded " + no6.ToString() + " times\n"
                + "CorrectRowsColsViaBlocks() succeeded " + no2.ToString() + " times\n"
                + "CorrectBlocksViaRowsCols() succeeded " + no3.ToString() + " times\n"
                + "HiddenSubset() succeeded " + no5.ToString() + " times\n");
                label1.Text = "Solved!";
            }

            if (filled < 81 && error == false)
            {
                TryToCheat();
            }

            if (error) { dp = dp + "e "; }
        }
        
        void tbTextChange(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string name = tb.Name;

            change = true;
            int num, i, j;
            int.TryParse(name.Substring(7), out num);
            i = (int)Math.Ceiling(((double)num) / 9);
            j = num - ((i - 1) * 9);
            if (!int.TryParse(tb.Text, out arr[i, j]))
            {
                tb.ResetText();
            }
            else
            {
                str[i, j] = "";
                correctNeighbors(i, j, arr[i, j]);
                tb.Enabled = false;
                tb.BackColor = Color.LightBlue;
                filled++;
            }
            load.Text = "Filled: "+filled.ToString();
        }

        void soleCandidate(int q)
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (str[i, j].Length == q)
                    {
                        if (q == 1)
                        {
                            getTb(i, j).Text = str[i, j];
                        }
                        else
                        {
                            getTb(i, j).BackColor = Color.Coral;
                        }
                    }
                }
            }
        }

        void correctNeighbors(int x, int y, int num)
        {
            //ROWS COLS
            for (int i = 1; i < 10; i++)
            {
                if (i != y && str[x, i].Contains(num.ToString()))
                {
                    strRemove(x,i,num);
                }
                if (i != x && str[i, y].Contains(num.ToString()))
                {
                    strRemove(i,y,num);
                }
            }

            //QUADS
            for (int i = x - 2; i < x + 3; i++)
            {
                if (i > 0 && i < 10)
                {
                    for (int j = y - 2; j < y + 3; j++)
                    {
                        if (j > 0 && j < 10 && getQuad(i, j) == getQuad(x, y) && str[i, j].Contains(num.ToString()))
                        {
                            strRemove(i, j, num);
                        }
                    }
                }
            }
        }
        
        //LAST

        void populate()
        {
            //str[,]
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    str[i, j] = "123456789";
                }
            }

            //block[,,]
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    int tb_num, a, b;
                    int.TryParse(getTb(i, j).Name.Substring(7), out tb_num);
                    Math.DivRem(j, 3, out b);
                    if (b == 0) { b = 3; }
                    Math.DivRem(i, 3, out a);
                    switch (a)
                    {
                        case 0: a = 2; break;
                        case 1: a = 0; break;
                        case 2: a = 1; break;
                    }
                    int c = 3 * a + b;

                    block[getQuad(i, j), c, 0] = i;
                    block[getQuad(i, j), c, 1] = j;
                }
            }
        }

        int getQuad(int i, int j)
        {
            int b = (int) Math.Ceiling(((double)j) / 3);
            int a = (int) (Math.Ceiling(((double)i) / 3))-1;
            return (a*3 + b);
        }
        
        TextBox getTb(int i, int j)
        {
            int num = i*9 - (9 - j);
            TextBox tb = (TextBox)Controls.Find("textBox" + num.ToString(), true)[0];
            return tb;
        }

        TextBox getTb(int num)
        {
            TextBox tb = (TextBox)Controls.Find("textBox" + num.ToString(), true)[0];
            return tb;
        }

        void strRemove(int i, int j, int num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num.ToString()), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                getTb(i, j).BackColor = Color.Red;
            }
        }

        void strRemove(int i, int j, char num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                getTb(i, j).BackColor = Color.Red;
            }
        }

        void strRemove(int i, int j, string num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                getTb(i, j).BackColor = Color.Red;
            }
        }

        void solve_Click(object sender, EventArgs e)
        {
            trySolve();
        }

        void load_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                load.BackColor = Color.White;
                load.Enabled = false;
                string file = openFileDialog1.FileName;
                try
                {
                    text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException) { }

                int counter = 0;
                int b;
                foreach (char a in text)
                {
                    if (int.TryParse(a.ToString(), out b) && counter < 81)
                    {
                        counter++;
                        if (b != 0)
                        {
                            getTb(counter).Text = b.ToString();
                        }
                    }
                }
            }
        }

        void tb_click(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int i, j, x;
            int.TryParse(tb.Name.Substring(7), out x);
            i = (int)Math.Ceiling(((double)x) / 9);
            j = x - ((i - 1) * 9);
            label1.Text = str[i, j].ToString();
        }
        
        void newWin_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dp.ToString());
            System.Diagnostics.Process.Start(Application.ExecutablePath);    //start new instance of application
            this.Close();
        }
    }
}