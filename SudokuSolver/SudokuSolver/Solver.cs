using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SudokuSolver
{
    partial class Form1
    {
        ///<summary>
        ///#0 (FILLS) Determines if there is a cell with only one possible entry
        ///</summary>
        void SoleCandidate()
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (str[i, j].Length == 1)
                    {
                        Fill(i, j, int.Parse(str[i, j]));
                        methodNo[0]++;
                    }
                }
            }
        }

        ///<summary>
        ///#1 (FILLS) Checks if only one cell in a row, column or block contains a certain digit as a possibility.
        ///</summary>
        void UniqueCandidate()
        {
            //CHECK BLOCKS
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
                                Fill(block[q, i, 0], block[q, i, 1], num);
                                methodNo[1]++;
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
                                Fill(i, j, num);
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
                                Fill(i, j, num);
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#2 (REMOVES) If only two cells in a block contain a digit and they share the same rown or column index, removes that digit from the candidates of the other cells in that row/column. 
        ///</summary>
        void PointingPair()
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
                            if (GetBlock(i, x) != q && str[i, x].Contains(n.ToString()))
                            {
                                methodNo[2]++;
                                StrRemove(i, x, n);
                            }
                        }
                    }
                    if (j > 0)
                    {
                        for (int x = 1; x < 10; x++)
                        {
                            if (GetBlock(x, j) != q && str[x, j].Contains(n.ToString()))
                            {
                                methodNo[2]++;
                                StrRemove(x, j, n);
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#3 (REMOVES) If only two cells of a row/column contain a certain digit and they are in the same block, removes that digit from the candidates of the other cells in that block.
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
                                q = GetBlock(i, j);
                            }
                            else
                            {
                                if (GetBlock(i, j) != q)
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
                                methodNo[3]++;
                                StrRemove(block[q, n, 0], block[q, n, 1], num);
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
                                q = GetBlock(i, j);
                            }
                            else
                            {
                                if (GetBlock(i, j) != q)
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
                                methodNo[3]++;
                                StrRemove(block[q, n, 0], block[q, n, 1], num);
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#4 (REMOVES) Checks if two connected cells contatin only two digits and if they are equal, trims these digits from the other connected cells.
        ///</summary>
        void NakedPair()
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
                                        methodNo[4]++;
                                        StrRemove(ctnr[a, 0], j, str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[ctnr[a, 0], j].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        methodNo[4]++;
                                        StrRemove(ctnr[a, 0], j, str[ctnr[a, 0], ctnr[a, 1]][1]);
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
                                        methodNo[4]++;
                                        StrRemove(i, ctnr[a, 1], str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[i, ctnr[a, 1]].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        methodNo[4]++;
                                        StrRemove(i, ctnr[a, 1], str[ctnr[a, 0], ctnr[a, 1]][1]);
                                    }
                                }
                            }
                        }
                        if (GetBlock(ctnr[a, 0], ctnr[a, 1]) == GetBlock(ctnr[b, 0], ctnr[b, 1]))
                        {
                            int q = GetBlock(ctnr[a, 0], ctnr[a, 1]);
                            for (int n = 1; n < 10; n++)
                            {
                                int i = block[q, n, 0];
                                int j = block[q, n, 1];
                                if (!(i == ctnr[a, 0] && j == ctnr[a, 1]) && !(i == ctnr[b, 0] && j == ctnr[b, 1]))
                                {
                                    if (str[i, j].Contains(str[ctnr[a, 0], ctnr[a, 1]][0]))
                                    {
                                        methodNo[4]++;
                                        StrRemove(i, j, str[ctnr[a, 0], ctnr[a, 1]][0]);
                                    }
                                    if (str[i, j].Contains(str[ctnr[a, 0], ctnr[a, 1]][1]))
                                    {
                                        methodNo[4]++;
                                        StrRemove(i, j, str[ctnr[a, 0], ctnr[a, 1]][1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///#5 (REWRITES) Checks if two connected cells are the only ones to have two specific candidates.
        ///If found, leaves these candidates as their only candidates.
        ///"change" variable must be handled, snice "strRemove" method is not used!
        ///</summary>
        void HiddenPair()
        {
            //ROWS
            for (int i = 1; i < 10; i++)
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
                                int j1 = 0;
                                for (int j = 1; j < 10; j++)
                                {
                                    if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
                                    {
                                        count++;
                                        if (count == 1) j1 = j;
                                        if (count == 2)
                                        {
                                            if (str[i, j1].Length > 2 || str[i, j].Length > 2)
                                            {
                                                str[i, j1] = str[i, j] = String.Concat(a, b);
                                                change = true;
                                                ///OLD BODY
                                                ///for (int num = 1; num < 10; num++)
                                                ///{
                                                ///    if (num != a && num != b)
                                                ///    {
                                                ///        if (str[i, j1].Contains(num.ToString()))
                                                ///        {
                                                ///            StrRemove(i, j1, num);
                                                ///            methodNo[5]++;
                                                ///        }
                                                ///        if (str[i, j].Contains(num.ToString()))
                                                ///        {
                                                ///            StrRemove(i, j, num);
                                                ///            methodNo[5]++;
                                                ///        }
                                                ///    }
                                                ///}
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //COLS
            for (int j = 1; j < 10; j++)
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
                                int i1 = 0;
                                for (int i = 1; i < 10; i++)
                                {
                                    if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
                                    {
                                        count++;
                                        if (count == 1) i1 = i;
                                        if (count == 2)
                                        {
                                            if (str[i1, j].Length > 2 || str[i, j].Length > 2)
                                            {
                                                str[i1, j] = str[i, j] = String.Concat(a, b);
                                                change = true;
                                                methodNo[5]++;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //BLOCKS
            for (int block = 1; block < 10; block++)
            {
                string concat = "";
                //inlined or not is better for speed?
                for (int n = 1; n < 10; n++)
                {
                    GetIndexes(block, n, out int i, out int j);
                    concat += str[i, j];
                }
                for (int a = 1; a < 10; a++)
                {
                    short count = 0;
                    foreach (char c in concat)
                    {
                        if (c.ToString() == a.ToString()) count++;
                    }
                    if (count == 2)
                    {
                        for(int b = a + 1; b < 10; b++)
                        {
                            count = 0;
                            foreach(char c in concat)
                            {
                                if (c.ToString() == b.ToString()) count++;
                            }
                            
                            if (count == 2)
                            {
                                count = 0;
                                int i, j, i1, j1;
                                i = j = i1 = j1 = 0;
                                for (int n = 1; n < 10; n++)
                                {
                                    GetIndexes(block, n, out i, out j);
                                    if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
                                    {
                                        count++;
                                        if (count == 1)
                                        {
                                            i1 = i;
                                            j1 = j;
                                        }
                                        if (count == 2)
                                        {
                                            if (str[i1, j1].Length > 2 || str[i, j].Length > 2)
                                            {
                                                str[i1, j1] = str[i, j] = String.Concat(a, b);
                                                change = true;
                                                methodNo[5]++;
                                            }
                                            break;
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
        ///#6 (partial)
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
                    if (GetBlock(ctnr[a, 0], ctnr[a, 1]) != GetBlock(ctnr[b, 0], ctnr[b, 1]))
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

        ///<summary>
        ///#6 (REMOVES) (partial)
        ///</summary>
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
                if ((GetBlock(i2, j2) == GetBlock(ctnr[x, 0], ctnr[x, 1])) && ctnr[x, 0] != i1 && ctnr[x, 1] != j1)
                {
                    if (str[ctnr[x, 0], ctnr[x, 1]].Contains(val2) && str[ctnr[x, 0], ctnr[x, 1]].Contains(eliminator))
                    {
                        //getTb(ctnr[x, 0], ctnr[x, 1]).BackColor = Color.Thistle;
                        if (i1 == i2)
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                if (str[ctnr[x, 0], j].Contains(eliminator) && GetBlock(ctnr[x, 0], j) == GetBlock(i1, j1))
                                {
                                    //getTb(ctnr[x, 0], j).BackColor = Color.Black;
                                    methodNo[6]++;
                                    StrRemove(ctnr[x, 0], j, eliminator);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                if (str[i, ctnr[x, 1]].Contains(eliminator) && GetBlock(i, ctnr[x, 1]) == GetBlock(i1, j1))
                                {
                                    //getTb(i, ctnr[x, 1]).BackColor = Color.Black;
                                    methodNo[6]++;
                                    StrRemove(i, ctnr[x, 1], eliminator);
                                }
                            }
                        }
                    }
                }
            }
        }

        ///<summary>
        ///This is a method that a human would not use to solve an expert level sudoku.
        ///It finds cells with two possible numbers left and fills one of them in, then tries to solve the sudoku again.
        ///Before that stores the puzzle progress to restore it in case of failure.
        ///On failure restores the sudoku before the filling of the cell and fills in the second possible number.
        ///It will try to guess the right number of as many cells as needed to solve the puzzle.
        ///If it fails to find a cell with exactly two candidates the sudoku won't be solved.
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

            if (i != 0)
            {
                int filledOld = filled;
                Array.Copy(arr, oldArr, arr.Length);
                Array.Copy(str, oldStr, str.Length);
                dp = dp + "\n(" + i.ToString() + j.ToString() + " - " + str[i, j][0] + ") ";
                Fill(i, j, int.Parse(str[i, j][0].ToString()));

                bool proceed = TryToSolve();

                if (filled < 81 || proceed)
                {
                    Restore(oldArr, oldStr, filledOld);
                    dp = dp + "\n(" + i.ToString() + j.ToString() + " - " + str[i, j][1] + ") ";
                    Fill(i, j, int.Parse(str[i, j][1].ToString()));
                    proceed = TryToSolve();
                }

                if (filled < 81 || proceed)
                {
                    Restore(oldArr, oldStr, filledOld);
                }
            }
            depth--;
        }

        void Restore(int[,] oldArr, string[,] oldStr, int filledOld)
        {
            filled = filledOld;
            error = false;
            Array.Copy(oldArr, arr, arr.Length);
            Array.Copy(oldStr, str, str.Length);
        }
    }
}
