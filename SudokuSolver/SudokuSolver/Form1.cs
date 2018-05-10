using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        int[] methodNo = { 0, 0, 0, 0, 0, 0, 0 };
        int filled, depth;
        //[i, j]
        int[,] arr = new int[10, 10];
        //[i, j]
        string[,] str = new string[10, 10];
        //[block_No, box_No, i_j_indexes]
        int[,,] block = new int[10, 10, 2];
        bool change, error;
        string dp;

        public Form1()
        {
            dp = "";
            filled = 0;
            depth = 0;
            error = false;
            InitializeComponent();
            Populate();
        }

        bool TryToSolve()
        {
            dp = dp + depth.ToString()+" ";
            do
            {
                change = false;
                SoleCandidate();
                UniqueCandidate();
                PointingPair();
                NakedPair();
                CorrectBlocksViaRowsCols();
                FindHook();
                HiddenPair();
            } while (change && filled != 81);

            if (filled == 81)
            {
                FillTextBoxes();
                label1.Text = "Solved!";
                dp += "\n\nSolved!\n";

                var findNextSolution = MessageBox.Show("Try to find another soluitons?", "Shall we continue?", MessageBoxButtons.YesNo);
                if (findNextSolution == DialogResult.Yes)
                {
                    label1.Text = "Working!";
                    //Render the label1 text first!
                    Application.DoEvents();
                    return true;
                }
                //MessageBox.Show(
                //"SoleCandidate() succeeded " + methodNo[0].ToString() + " times\n"
                //+ "UniqueCandidate() succeeded " + methodNo[1].ToString() + " times\n"
                //+ "NakedSubset() succeeded " + methodNo[4].ToString() + " times\n"
                //+ "CorrectRowsColsViaBlocks() succeeded " + methodNo[2].ToString() + " times\n"
                //+ "CorrectBlocksViaRowsCols() succeeded " + methodNo[3].ToString() + " times\n"
                //+ "HiddenSubset() succeeded " + methodNo[5].ToString() + " times\n"
                //+ "FindHook() succeeded " + methodNo[6].ToString() + " times\n");
            }
            else if (error == false)
            {
                TryToCheat();
            }
            else
            {
                dp = dp + "e ";
                label1.Text = "No solution found!";
            }
            return false;
        }

        //Fills the array values into the text boxes after trySolve() has finished
        void FillTextBoxes()
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (arr[i, j] > 0)
                    {
                        GetTextBox(i, j).Text = arr[i, j].ToString();
                    }
                }
            }
        }
        
        //Fills a value in arr[i,j]
        void Fill(int i, int j, int num)
        {
            arr[i,j] = num;
            str[i, j] = "";
            filled++;
            CorrectNeighbors(i, j, arr[i, j]);
            change = true;
        }

        //Executes on any TextBox.Text change event
        void TextBoxTextChange(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            GetTextBoxIndexes(tb, out int i, out int j);
            if (!int.TryParse(tb.Text, out int num))
            {
                tb.ResetText();
            }
            else
            {
                if(arr[i, j] < 1) Fill(i, j, num);
                tb.Enabled = false;
                tb.BackColor = Color.LightBlue;
            }
            load.Text = "Filled: "+filled.ToString();
        }

        //(REMOVES) Executes for every resolved cell
        void CorrectNeighbors(int x, int y, int num)
        {
            //ROWS COLS
            for (int i = 1; i < 10; i++)
            {
                if (i != y && str[x, i].Contains(num.ToString()))
                {
                    StrRemove(x,i,num);
                }
                if (i != x && str[i, y].Contains(num.ToString()))
                {
                    StrRemove(i,y,num);
                }
            }

            //QUADS
            for (int i = x - 2; i < x + 3; i++)
            {
                if (i > 0 && i < 10)
                {
                    for (int j = y - 2; j < y + 3; j++)
                    {
                        if (j > 0 && j < 10 && GetBlock(i, j) == GetBlock(x, y) && str[i, j].Contains(num.ToString()))
                        {
                            StrRemove(i, j, num);
                        }
                    }
                }
            }
        }

        //Fills the initial values of some variables
        void Populate()
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
                    int.TryParse(GetTextBox(i, j).Name.Substring(7), out tb_num);
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

                    block[GetBlock(i, j), c, 0] = i;
                    block[GetBlock(i, j), c, 1] = j;
                }
            }
        }

        //Gets the number of the block that holds the cell with the given indexes
        int GetBlock(int i, int j)
        {
            int b = (int) Math.Ceiling(((double)j) / 3);
            int a = (int) (Math.Ceiling(((double)i) / 3))-1;
            return (a*3 + b);
        }

        //Gets the indexes of a cell, given its block number (1-9) and its number in the block (1-9)
        void GetIndexes(int b, int n, out int i, out int j)
        {
            i = (int)Math.Ceiling((double)b / 3) * 3 - 2;
            i += (int)Math.Ceiling((double)n / 3) - 1;

            Math.DivRem(b, 3, out j);
            if (j == 0) j = 7;
            if (j == 2) j = 4;
            Math.DivRem(n, 3, out n);
            if (n > 0) n -= 1;
            else n += 2;

            j += n;
        }

        //Finds the indexes of the given TextBox object
        void GetTextBoxIndexes(TextBox tb, out int i, out int j)
        {
            double.TryParse(tb.Name.Substring(7), out double num);
            i = (int)Math.Ceiling(num / 9d);
            j = Convert.ToInt32(num - ((i - 1) * 9));
        }

        //Gets a TextBox object given its indexes
        TextBox GetTextBox(int i, int j)
        {
            int num = i*9 - (9 - j);
            TextBox tb = (TextBox)Controls.Find("textBox" + num.ToString(), true)[0];
            return tb;
        }

        //Gets a TextBox object given its sequential number
        TextBox GetTextBox(int num)
        {
            TextBox tb = (TextBox)Controls.Find("textBox" + num.ToString(), true)[0];
            return tb;
        }

        void StrRemove(int i, int j, int num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num.ToString()), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                //GetTextBox(i, j).BackColor = Color.Red;
            }
        }

        void StrRemove(int i, int j, char num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                //GetTextBox(i, j).BackColor = Color.Red;
            }
        }

        void StrRemove(int i, int j, string num)
        {
            str[i, j] = str[i, j].Remove(str[i, j].IndexOf(num), 1);
            change = true;
            if (str[i, j].Length < 1)
            {
                error = true;
                //GetTextBox(i, j).BackColor = Color.Red;
            }
        }

        void solve_Click(object sender, EventArgs e)
        {
            label1.Text = "Working!";
            //Render the label1 text first!
            Application.DoEvents();
            TryToSolve();
        }

        void load_Click(object sender, EventArgs e)
        {
            string text = "";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    string file = openFileDialog1.FileName;
                    text = File.ReadAllText(file);
                    text = text.Replace("\n", String.Empty);
                    text = text.Replace("\r", String.Empty);
                    text = text.Replace("\t", String.Empty);
                    load.Enabled = false;
                    load.BackColor = Color.White;
                }
                catch (IOException ex) { MessageBox.Show("The file was not properly loaded.\n Error: " + ex.ToString()); }

                int counter = 0;
                bool symbolEncountered = false;
                foreach (char a in text)
                {
                    if (int.TryParse(a.ToString(), out int b))
                    {
                        counter++;
                        if (b != 0)
                        {
                            GetTextBox(counter).Text = b.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show(text);
                        symbolEncountered = true;
                    }
                }
                if(counter != 81 && symbolEncountered) MessageBox.Show("The length of the file is different from 81.\nA symbol different than a number was encountered.\nMake sure that the sudoku is introduced correctly!");
                else if (counter != 81) MessageBox.Show("The length of the file is different from 81.\nMake sure that the sudoku is introduced correctly!");
                else if (symbolEncountered) MessageBox.Show("A symbol different than a number was encountered.\nMake sure that the sudoku is introduced correctly!");
            }
        }

        void tb_click(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GetTextBoxIndexes(tb, out int i, out int j);
            label1.Text = str[i, j].ToString();
        }
        
        void newWin_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dp.ToString());
            System.Diagnostics.Process.Start(Application.ExecutablePath);    //start new instance of application
            this.Close();
        }
    }
}