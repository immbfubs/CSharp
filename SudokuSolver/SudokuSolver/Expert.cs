using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        ///<summary>
        ///#1 (FILLS) Checks if only one cell in a row, column or block contains a digit as a possibility.
        ///</summary>
        void UniqueCandidate()
        {
            //CHECK QUADS
            for (int q = 1; q < 10; q++)
            {
                string chain = "";

                for (int n = 1; n < 10; n++)
                {
                    chain = chain + str[block[q, n, 0], block[q, n, 1]];
                }

                for (int num = 1; num < 10; num++)
                {
                    int idx = chain.IndexOf(num.ToString());
                    if (idx != -1 && chain.IndexOf(num.ToString(), idx + 1) == -1)
                    {
                        for (int i = 1; i < 10; i++)
                        {
                            if (str[block[q, i, 0], block[q, i, 1]].Contains(num.ToString()))
                            {
                                getTb(block[q, i, 0], block[q, i, 1]).Text = num.ToString();
                            }
                        }
                    }
                }
            }

            //CHECK ROWS
            for (int i = 1; i < 10; i++)
            {
                string chain = "";

                for (int j = 1; j < 10; j++)
                {
                    chain = chain + str[i, j];
                }

                for (int num = 1; num < 10; num++)
                {
                    int idx = chain.IndexOf(num.ToString());
                    if (idx != -1 && chain.IndexOf(num.ToString(), idx + 1) == -1)
                    {
                        for (int j = 1; j < 10; j++)
                        {
                            if (str[i, j].Contains(num.ToString()))
                            {
                                getTb(i, j).Text = num.ToString();
                            }
                        }
                    }
                }
            }

            //CHECK COLS
            for (int j = 1; j < 10; j++)
            {
                string chain = "";

                for (int i = 1; i < 10; i++)
                {
                    chain = chain + str[i, j];
                }

                for (int num = 1; num < 10; num++)
                {
                    int idx = chain.IndexOf(num.ToString());
                    if (idx != -1 && chain.IndexOf(num.ToString(), idx + 1) == -1)
                    {
                        for (int i = 1; i < 10; i++)
                        {
                            if (str[i, j].Contains(num.ToString()))
                            {
                                getTb(i, j).Text = num.ToString();
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#2 (REMOVES) If only two cells in a block contain a digit and they have the same rown or column index, removes that digit from the possibilities for the other cells in that row/column. 
        ///</summary>
        void CorrectRowsColsViaBlocks()
        {
            for (int q = 1; q < 10; q++)
            {
                for (int n = 1; n < 10; n++)
                {
                    int i = 0, j = 0;
                    for (int box = 1; box < 10; box++)
                    {
                        if (str[block[q, box, 0], block[q, box, 1]].Contains(n.ToString()))
                        {
                            if (i == 0)
                            {
                                i = block[q, box, 0];
                                j = block[q, box, 1];
                            }
                            else
                            {
                                if (i != block[q, box, 0]) { i = -1; }
                                if (j != block[q, box, 1]) { j = -1; }
                            }
                        }
                    }
                    if (i > 0)
                    {
                        for (int x = 1; x < 10; x++)
                        {
                            if (getQuad(i, x) != q && str[i, x].Contains(n.ToString()))
                            {
                                no2++;
                                strRemove(i, x, n);
                            }
                        }
                    }
                    if (j > 0)
                    {
                        for (int x = 1; x < 10; x++)
                        {
                            if (getQuad(x, j) != q && str[x, j].Contains(n.ToString()))
                            {
                                no2++;
                                strRemove(x, j, n);
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#3 (REMOVES) If only two cells of a row/column contain a digit and they are in the same block, removes that digit from the possibilities of the other cells in that block.
        ///</summary>
        void CorrectBlocksViaRowsCols()
        {
            int q, counter;
            bool sameQuad;

            //ROWS
            for (int num = 1; num < 10; num++)
            {
                for (int i = 1; i < 10; i++)
                {
                    q = counter = 0;
                    sameQuad = true;
                    for (int j = 1; j < 10; j++)
                    {
                        if (str[i, j].Contains(num.ToString()))
                        {
                            counter++;
                            if (q == 0)
                            {
                                q = getQuad(i, j);
                            }
                            else
                            {
                                if (getQuad(i, j) != q)
                                {
                                    sameQuad = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (sameQuad && counter > 1)
                    {
                        for (short n = 1; n < 10; n++)
                        {
                            if (block[q, n, 0] != i && str[block[q, n, 0], block[q, n, 1]].Contains(num.ToString()))
                            {
                                no3++;
                                strRemove(block[q, n, 0], block[q, n, 1], num);
                            }
                        }
                    }
                }

                //COLS
                for (int j = 1; j < 10; j++)
                {
                    q = counter = 0;
                    sameQuad = true;
                    for (int i = 1; i < 10; i++)
                    {
                        if (str[i, j].Contains(num.ToString()))
                        {
                            counter++;
                            if (i == 1)
                            {
                                q = getQuad(i, j);
                            }
                            else
                            {
                                if (getQuad(i, j) != q)
                                {
                                    sameQuad = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (sameQuad && counter > 1)
                    {
                        for (short n = 1; n < 10; n++)
                        {
                            if (block[q, n, 1] != j && str[block[q, n, 0], block[q, n, 1]].Contains(num.ToString()))
                            {
                                no3++;
                                strRemove(block[q, n, 0], block[q, n, 1], num);
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#4 (REMOVES) Checks if two connected boxes contatin only two digits and if they are the same, trims these digits from the other connected boxes.
        ///</summary>
        void NakedSubset()
        {
            int[,] ctnr = new int[81, 2];
            int counter = 0;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (str[i, j].Length == 2)
                    {
                        ctnr[counter, 0] = i;
                        ctnr[counter, 1] = j;
                        counter++;
                    }
                }
            }
            for (int a = 0; a < counter; a++)
            {
                for (int b = 0; b < counter; b++)
                {
                    if (str[ctnr[a, 0], ctnr[a, 1]] == str[ctnr[b, 0], ctnr[b, 1]] && !(ctnr[a, 0] == ctnr[b, 0] && ctnr[a, 1] == ctnr[b, 1]) && str[ctnr[a, 0], ctnr[a, 1]].Length > 1)
                    {
                        if (ctnr[a, 0] == ctnr[b, 0])
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                if (j != ctnr[a, 1] && j != ctnr[b, 1])
                                {
                                    if (str[ctnr[a, 0], j].Contains(str[ctnr[a, 0], ctnr[a, 1]][0]))
                                    {
                                        no4++;
                                        strRemove(ctnr[a, 0], j, str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[ctnr[a, 0], j].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        no4++;
                                        strRemove(ctnr[a, 0], j, str[ctnr[a, 0], ctnr[a, 1]][1]);
                                    }
                                }
                            }
                        }
                        if (ctnr[a, 1] == ctnr[b, 1])
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                if (i != ctnr[a, 0] && i != ctnr[b, 0])
                                {
                                    if (str[i, ctnr[a, 1]].Contains(str[ctnr[a, 0], ctnr[a, 1]][0]))
                                    {
                                        no4++;
                                        strRemove(i, ctnr[a, 1], str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[i, ctnr[a, 1]].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        no4++;
                                        strRemove(i, ctnr[a, 1], str[ctnr[a, 0], ctnr[a, 1]][1]);
                                    }
                                }
                            }
                        }
                        if (getQuad(ctnr[a, 0], ctnr[a, 1]) == getQuad(ctnr[b, 0], ctnr[b, 1]))
                        {
                            int q = getQuad(ctnr[a, 0], ctnr[a, 1]);
                            for (int n = 1; n < 10; n++)
                            {
                                int i = block[q, n, 0];
                                int j = block[q, n, 1];
                                if (!(i == ctnr[a, 0] && j == ctnr[a, 1]) && !(i == ctnr[b, 0] && j == ctnr[b, 1]))
                                {
                                    if (str[i, j].Contains(str[ctnr[a, 0], ctnr[a, 1]][0]))
                                    {
                                        no4++;
                                        strRemove(i, j, str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[i, j].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        no4++;
                                        strRemove(i, j, str[ctnr[a, 0], ctnr[a, 1]][1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#5 (REMOVES) Checks if two connected boxes are the only one to contain two digits and trims the other possible digits
        ///</summary>
        void HiddenSubset()
        {
            for (int i = 1; i < 10; i++)
            //ROWS
            {
                string concat = "";
                for (int j = 1; j < 10; j++) concat += str[i, j];
                for (int a = 1; a < 10; a++)
                {
                    short count = 0;
                    foreach (char c in concat)
                    {
                        if (int.Parse(c.ToString()) == a) count++;
                    }
                    if (count == 2)
                    {
                        for (int b = a + 1; b < 10; b++)
                        {
                            count = 0;
                            foreach (char c in concat)
                            {
                                if (int.Parse(c.ToString()) == b) count++;
                            }
                            if (count == 2)
                            {
                                count = 0;
                                int j1, j2;
                                j1 = j2 = 0;
                                for (int j = 1; j < 10; j++)
                                {
                                    if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
                                    {
                                        if (count == 0) j1 = j;
                                        if (count == 1) j2 = j;
                                        count++;
                                    }
                                }
                                if (count == 2)
                                {
                                    for (int num = 1; num < 10; num++)
                                    {
                                        if (num != a && num != b)
                                        {
                                            if (str[i, j1].Contains(num.ToString()))
                                            {
                                                strRemove(i, j1, num);
                                                no5++;
                                            }
                                            if (str[i, j2].Contains(num.ToString()))
                                            {
                                                strRemove(i, j2, num);
                                                no5++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int j = 1; j < 10; j++)
            //COLS
            {
                string concat = "";
                for (int i = 1; i < 10; i++) concat += str[i, j];
                for (int a = 1; a < 10; a++)
                {
                    short count = 0;
                    foreach (char c in concat)
                    {
                        if (int.Parse(c.ToString()) == a) count++;
                    }
                    if (count == 2)
                    {
                        for (int b = a + 1; b < 10; b++)
                        {
                            count = 0;
                            foreach (char c in concat)
                            {
                                if (int.Parse(c.ToString()) == b) count++;
                            }
                            if (count == 2)
                            {
                                count = 0;
                                int i1, i2;
                                i1 = i2 = 0;
                                for (int i = 1; i < 10; i++)
                                {
                                    if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
                                    {
                                        if (count == 0) i1 = i;
                                        if (count == 1) i2 = i;
                                        count++;
                                    }
                                }
                                if (count == 2)
                                {
                                    for (int num = 1; num < 10; num++)
                                    {
                                        if (num != a && num != b)
                                        {
                                            if (str[i1, j].Contains(num.ToString()))
                                            {
                                                strRemove(i1, j, num);
                                                no5++;
                                            }
                                            if (str[i2, j].Contains(num.ToString()))
                                            {
                                                strRemove(i2, j, num);
                                                no5++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#6 (REMOVES)
        ///</summary>
        void FindHook()
        {
            // #5
            int[,] ctnr = new int[81, 2];
            int counter = 0;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (str[i, j].Length == 2)
                    {
                        ctnr[counter, 0] = i;
                        ctnr[counter, 1] = j;
                        counter++;
                    }
                }
            }

            for (int a = 0; a < counter + 1; a++)
            {
                for (int b = 0; b < counter + 1; b++)
                {
                    if (getQuad(ctnr[a, 0], ctnr[a, 1]) != getQuad(ctnr[b, 0], ctnr[b, 1]))
                    {
                        if (ctnr[a, 0] == ctnr[b, 0] || ctnr[a, 1] == ctnr[b, 1])
                        {
                            if (str[ctnr[a, 0], ctnr[a, 1]] != str[ctnr[b, 0], ctnr[b, 1]] && str[ctnr[a, 0], ctnr[a, 1]].Length == 2 && str[ctnr[b, 0], ctnr[b, 1]].Length == 2)
                            {
                                if (str[ctnr[a, 0], ctnr[a, 1]].Contains(str[ctnr[b, 0], ctnr[b, 1]][0]))
                                {
                                    FindHookPoint(ctnr[a, 0], ctnr[a, 1], ctnr[b, 0], ctnr[b, 1], str[ctnr[b, 0], ctnr[b, 1]][0], ref ctnr, ref counter);
                                }
                                if (str[ctnr[a, 0], ctnr[a, 1]].Contains(str[ctnr[b, 0], ctnr[b, 1]][1]))
                                {
                                    FindHookPoint(ctnr[a, 0], ctnr[a, 1], ctnr[b, 0], ctnr[b, 1], str[ctnr[b, 0], ctnr[b, 1]][0], ref ctnr, ref counter);
                                }
                            }
                        }
                    }
                }
            }
        }

        void FindHookPoint(int i1, int j1, int i2, int j2, char val1, ref int[,] ctnr, ref int counter)
        {
            // findHook() extention
            char val2 = 'a', eliminator = 'a';
            switch (str[i2, j2].IndexOf(val1))
            {
                case 0: val2 = str[i2, j2][1]; break;
                case 1: val2 = str[i2, j2][0]; break;
            }
            switch (str[i1, j1].IndexOf(val1))
            {
                case 0: eliminator = str[i1, j1][1]; break;
                case 1: eliminator = str[i1, j1][0]; break;
            }

            for (int x = 0; x < counter; x++)
            {
                if ((getQuad(i2, j2) == getQuad(ctnr[x, 0], ctnr[x, 1])) && ctnr[x, 0] != i1 && ctnr[x, 1] != j1)
                {
                    if (str[ctnr[x, 0], ctnr[x, 1]].Contains(val2) && str[ctnr[x, 0], ctnr[x, 1]].Contains(eliminator))
                    {
                        //getTb(ctnr[x, 0], ctnr[x, 1]).BackColor = Color.Thistle;
                        if (i1 == i2)
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                if (str[ctnr[x, 0], j].Contains(eliminator) && getQuad(ctnr[x, 0], j) == getQuad(i1, j1))
                                {
                                    //getTb(ctnr[x, 0], j).BackColor = Color.Black;
                                    no6++;
                                    strRemove(ctnr[x, 0], j, eliminator);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                if (str[i, ctnr[x, 1]].Contains(eliminator) && getQuad(i, ctnr[x, 1]) == getQuad(i1, j1))
                                {
                                    //getTb(i, ctnr[x, 1]).BackColor = Color.Black;
                                    no6++;
                                    strRemove(i, ctnr[x, 1], eliminator);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Cheater

        ///<summary>
        ///Search for cells with two possible digits and fills one of them them enters recursion while storing the needed data to restore the puzzle on failure. On failure tries the other digit.
        ///</summary>
        void TryToCheat()
        {
            depth++;
            int i = 0;
            int j = 0;
            string[,] oldStr = new string[10, 10];
            int[,] oldArr = new int[10, 10];
            for (int k = 1; k < 10; k++)
            {
                for (int l = 1; l < 10 && i == 0; l++)
                {
                    if (str[k, l].Length == 2)
                    {
                        i = k;
                        j = l;
                    }
                }
            }
            dp = dp + "\n("+i.ToString()+j.ToString()+") ";
            if (i != 0)
            {
                int filledOld = filled;
                Array.Copy(arr, oldArr, arr.Length);
                Array.Copy(str, oldStr, str.Length);
                getTb(i, j).Text = str[i, j][0].ToString();

                trySolve();

                if (filled < 81)
                {
                    Restore(oldArr, oldStr, filledOld);
                    getTb(i, j).Text = str[i, j][1].ToString();
                    trySolve();
                }

                if (filled < 81)
                {
                    Restore(oldArr, oldStr, filledOld);
                }
            }
            depth--;
        }

        void Restore(int[,] oldArr, string[,] oldStr, int filledOld)
        {
            foreach (Control x in this.Controls)
            {
                if (x is TextBox)
                {
                    ((TextBox)x).Enabled = true;
                    ((TextBox)x).ResetText();
                    ((TextBox)x).BackColor = Color.White;
                }
            }
            filled = 0;
            error = false;
            Array.Copy(oldArr, arr, arr.Length);
            Array.Copy(oldStr, str, str.Length);
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (arr[i, j] > 0)
                    {
                        getTb(i, j).Text = arr[i, j].ToString();
                    }
                }
            }
        }
    }
}