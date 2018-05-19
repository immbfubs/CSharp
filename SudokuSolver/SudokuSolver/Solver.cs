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
                    if (candidates[i, j, 0] == 1)
                    {
                        Fill(i, j, candidates.Last(i, j));
                    }
                }
            }
        }

        ///<summary>
        ///#1 (FILLS) Checks if only one cell in a row, column or block contains a certain digit as a possibility.
        ///</summary>
        void UniqueCandidate()
        {
            lastMethod = 1;
            //CHECK BLOCKS
            for (int q = 1; q < 10; q++)
            {
                for (int cand = 1; cand < 10; cand++)
                {
                    byte counter = 0;
                    for(int cell = 1; cell < 10; cell++)
                    {
                        if (candidates[block[q, cell, 0], block[q, cell, 1], cand] == 0) counter++;
                    }

                    if(counter == 1)
                    {
                        for (int cell = 1; cell < 10; cell++)
                        {
                            if (candidates[block[q, cell, 0], block[q, cell, 1], cand] == 0)
                            {
                                Fill(block[q, cell, 0], block[q, cell, 1], cand);
                                methodNo[1]++;
                            }
                        }
                    }
                }
            }

            //CHECK ROWS
            for (int i = 1; i < 10; i++)
            {
                for (int cand = 1; cand < 10; cand++)
                {
                    byte counter = 0;
                    for (int j = 1; j < 10; j++)
                    {
                        if (candidates[i, j, cand] == 0) counter++;
                    }

                    if (counter == 1)
                    {
                        for (int j = 1; j < 10; j++)
                        {
                            if (candidates[i, j, cand] == 0)
                            {
                                Fill(i, j, cand);
                                methodNo[1]++;
                            }
                        }
                    }
                }
            }

            //CHECK COLS
            for (int j = 1; j < 10; j++)
            {
                for (int cand = 1; cand < 10; cand++)
                {
                    byte counter = 0;
                    for (int i = 1; i < 10; i++)
                    {
                        if (candidates[i, j, cand] == 0) counter++;
                    }

                    if (counter == 1)
                    {
                        for (int i = 1; i < 10; i++)
                        {
                            if (candidates[i, j, cand] == 0)
                            {
                                Fill(i, j, cand);
                                methodNo[1]++;
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
            lastMethod = 2;
            for (int q = 1; q < 10; q++)
            {
                for (int n = 1; n < 10; n++)
                {
                    int i = 0, j = 0;
                    for (int box = 1; box < 10; box++)
                    {
                        if (candidates[block[q, box, 0], block[q, box, 1], n] == 0)
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
                            if (GetBlock(i, x) != q && candidates[i, x, n] == 0)
                            {
                                methodNo[2]++;
                                RemoveCandidate(i, x, n);
                            }
                        }
                    }
                    if (j > 0)
                    {
                        for (int x = 1; x < 10; x++)
                        {
                            if (GetBlock(x, j) != q && candidates[x, j, n] == 0)
                            {
                                methodNo[2]++;
                                RemoveCandidate(x, j, n);
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
            lastMethod = 3;
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
                        if (candidates[i, j, num] == 0)
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
                            if (block[q, n, 0] != i && candidates[block[q, n, 0], block[q, n, 1], num] == 0)
                            {
                                methodNo[3]++;
                                RemoveCandidate(block[q, n, 0], block[q, n, 1], num);
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
                        if (candidates[i, j, num] == 0)
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
                            if (block[q, n, 1] != j && candidates[block[q, n, 0], block[q, n, 1], num] == 0)
                            {
                                methodNo[3]++;
                                RemoveCandidate(block[q, n, 0], block[q, n, 1], num);
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
            lastMethod = 4;
            int[,] ctnr = new int[81, 2];
            int counter = 0;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (candidates[i, j, 0] == 2)
                    {
                        ctnr[counter, 0] = i;
                        ctnr[counter, 1] = j;
                        counter++;
                    }
                }
            }
            for (int c1 = 0; c1 < counter; c1++)
            {
                for (int c2 = 0; c2 < counter; c2++)
                {
                    if ((ctnr[c1, 0] != ctnr[c2, 0] || ctnr[c1, 1] != ctnr[c2, 1]) && candidates.AreEqual(ctnr[c1, 0], ctnr[c1, 1], ctnr[c2, 0], ctnr[c2, 1]) && candidates[ctnr[c1, 0], ctnr[c1, 1], 0] > 1)
                    {
                        int[] both = candidates.ToArray(ctnr[c1, 0], ctnr[c1, 1]);
                        if (ctnr[c1, 0] == ctnr[c2, 0])
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                if (j != ctnr[c1, 1] && j != ctnr[c2, 1])
                                {
                                    if (candidates[ctnr[c1, 0], j, both[0]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(ctnr[c1, 0], j, both[0]);
                                    }
                                    if (candidates[ctnr[c1, 0], j, both[1]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(ctnr[c1, 0], j, both[1]);
                                    }
                                }
                            }
                        }
                        else if (ctnr[c1, 1] == ctnr[c2, 1])
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                if (i != ctnr[c1, 0] && i != ctnr[c2, 0])
                                {
                                    if (candidates[i, ctnr[c1, 1], both[0]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(i, ctnr[c1, 1], both[0]);
                                    }
                                    if (candidates[i, ctnr[c1, 1], both[1]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(i, ctnr[c1, 1], both[1]);
                                    }
                                }
                            }
                        }
                        if (GetBlock(ctnr[c1, 0], ctnr[c1, 1]) == GetBlock(ctnr[c2, 0], ctnr[c2, 1]))
                        {
                            int q = GetBlock(ctnr[c1, 0], ctnr[c1, 1]);
                            for (int n = 1; n < 10; n++)
                            {
                                int i = block[q, n, 0];
                                int j = block[q, n, 1];
                                if ((i != ctnr[c1, 0] || j != ctnr[c1, 1]) && (i != ctnr[c2, 0] || j != ctnr[c2, 1]))
                                {
                                    if (candidates[i, j, both[0]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(i, j, both[0]);
                                    }
                                    if (candidates[i, j, both[1]] == 0)
                                    {
                                        methodNo[4]++;
                                        RemoveCandidate(i, j, both[1]);
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
        //void HiddenPair()
        //{
        //    lastMethod = 5;
        //    //ROWS
        //    for (int i = 1; i < 10; i++)
        //    {
        //        string concat = "";
        //        for (int j = 1; j < 10; j++) concat += str[i, j];
        //        for (int a = 1; a < 10; a++)
        //        {
        //            short count = 0;
        //            foreach (char c in concat)
        //            {
        //                if (int.Parse(c.ToString()) == a) count++;
        //            }
        //            if (count == 2)
        //            {
        //                for (int b = a + 1; b < 10; b++)
        //                {
        //                    count = 0;
        //                    foreach (char c in concat)
        //                    {
        //                        if (int.Parse(c.ToString()) == b) count++;
        //                    }
        //                    if (count == 2)
        //                    {
        //                        count = 0;
        //                        int j1 = 0;
        //                        for (int j = 1; j < 10; j++)
        //                        {
        //                            if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
        //                            {
        //                                count++;
        //                                if (count == 1) j1 = j;
        //                                if (count == 2)
        //                                {
        //                                    if (str[i, j1].Length > 2 || str[i, j].Length > 2)
        //                                    {
        //                                        str[i, j1] = str[i, j] = String.Concat(a, b);
        //                                        change = true;
        //                                        ///OLD BODY
        //                                        ///for (int num = 1; num < 10; num++)
        //                                        ///{
        //                                        ///    if (num != a && num != b)
        //                                        ///    {
        //                                        ///        if (str[i, j1].Contains(num.ToString()))
        //                                        ///        {
        //                                        ///            StrRemove(i, j1, num);
        //                                        ///            methodNo[5]++;
        //                                        ///        }
        //                                        ///        if (str[i, j].Contains(num.ToString()))
        //                                        ///        {
        //                                        ///            StrRemove(i, j, num);
        //                                        ///            methodNo[5]++;
        //                                        ///        }
        //                                        ///    }
        //                                        ///}
        //                                    }
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //COLS
        //    for (int j = 1; j < 10; j++)
        //    {
        //        string concat = "";
        //        for (int i = 1; i < 10; i++) concat += str[i, j];
        //        for (int a = 1; a < 10; a++)
        //        {
        //            short count = 0;
        //            foreach (char c in concat)
        //            {
        //                if (int.Parse(c.ToString()) == a) count++;
        //            }
        //            if (count == 2)
        //            {
        //                for (int b = a + 1; b < 10; b++)
        //                {
        //                    count = 0;
        //                    foreach (char c in concat)
        //                    {
        //                        if (int.Parse(c.ToString()) == b) count++;
        //                    }
        //                    if (count == 2)
        //                    {
        //                        count = 0;
        //                        int i1 = 0;
        //                        for (int i = 1; i < 10; i++)
        //                        {
        //                            if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
        //                            {
        //                                count++;
        //                                if (count == 1) i1 = i;
        //                                if (count == 2)
        //                                {
        //                                    if (str[i1, j].Length > 2 || str[i, j].Length > 2)
        //                                    {
        //                                        str[i1, j] = str[i, j] = String.Concat(a, b);
        //                                        change = true;
        //                                        methodNo[5]++;
        //                                    }
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //BLOCKS
        //    for (int block = 1; block < 10; block++)
        //    {
        //        string concat = "";
        //        //inlined or not is better for speed?
        //        for (int n = 1; n < 10; n++)
        //        {
        //            GetIndexes(block, n, out int i, out int j);
        //            concat += str[i, j];
        //        }
        //        for (int a = 1; a < 10; a++)
        //        {
        //            short count = 0;
        //            foreach (char c in concat)
        //            {
        //                if (c.ToString() == a.ToString()) count++;
        //            }
        //            if (count == 2)
        //            {
        //                for(int b = a + 1; b < 10; b++)
        //                {
        //                    count = 0;
        //                    foreach(char c in concat)
        //                    {
        //                        if (c.ToString() == b.ToString()) count++;
        //                    }
                            
        //                    if (count == 2)
        //                    {
        //                        count = 0;
        //                        int i, j, i1, j1;
        //                        i = j = i1 = j1 = 0;
        //                        for (int n = 1; n < 10; n++)
        //                        {
        //                            GetIndexes(block, n, out i, out j);
        //                            if (str[i, j].Contains(a.ToString()) && str[i, j].Contains(b.ToString()))
        //                            {
        //                                count++;
        //                                if (count == 1)
        //                                {
        //                                    i1 = i;
        //                                    j1 = j;
        //                                }
        //                                if (count == 2)
        //                                {
        //                                    if (str[i1, j1].Length > 2 || str[i, j].Length > 2)
        //                                    {
        //                                        str[i1, j1] = str[i, j] = String.Concat(a, b);
        //                                        change = true;
        //                                        methodNo[5]++;
        //                                    }
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        ///<summary>
        ///#6 (partial)
        ///</summary>
        void FindHook()
        {
            lastMethod = 6;
            // #5
            int[,] ctnr = new int[81, 2];
            int counter = 0;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (candidates[i, j, 0] == 2)
                    {
                        ctnr[counter, 0] = i;
                        ctnr[counter, 1] = j;
                        counter++;
                    }
                }
            }

            for (int c1 = 0; c1 < counter + 1; c1++)
            {
                for (int c2 = 0; c2 < counter + 1; c2++)
                {
                    if (GetBlock(ctnr[c1, 0], ctnr[c1, 1]) != GetBlock(ctnr[c2, 0], ctnr[c2, 1]))
                    {
                        if (ctnr[c1, 0] == ctnr[c2, 0] || ctnr[c1, 1] == ctnr[c2, 1])
                        {
                            if (!candidates.AreEqual(ctnr[c1, 0], ctnr[c1, 1], ctnr[c2, 0], ctnr[c2, 1]) && candidates[ctnr[c1, 0], ctnr[c1, 1], 0] == 2 && candidates[ctnr[c2, 0], ctnr[c2, 1], 0] == 2)
                            {
                                int[] c2Candidates = candidates.ToArray(ctnr[c2, 0], ctnr[c2, 1]);
                                if (candidates[ctnr[c1, 0], ctnr[c1, 1], c2Candidates[0]] == 0)
                                {
                                    FindHookPoint(ctnr[c1, 0], ctnr[c1, 1], ctnr[c2, 0], ctnr[c2, 1], c2Candidates[0], ref ctnr, ref counter);
                                }
                                if (candidates[ctnr[c1, 0], ctnr[c1, 1], c2Candidates[1]] == 0)
                                {
                                    FindHookPoint(ctnr[c1, 0], ctnr[c1, 1], ctnr[c2, 0], ctnr[c2, 1], c2Candidates[1], ref ctnr, ref counter);
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
        void FindHookPoint(int i1, int j1, int i2, int j2, int val1, ref int[,] ctnr, ref int counter)
        {
            lastMethod = 7;
            int[] c1Cds = candidates.ToArray(i1, j1);
            int[] c2Cds = candidates.ToArray(i2, j2);
            int val2 = -1, eliminator = -1;

            switch (Array.IndexOf(c2Cds, val1))
            {
                case 0: val2 = c2Cds[1]; break;
                case 1: val2 = c2Cds[0]; break;
            }
            switch (Array.IndexOf(c1Cds, val1))
            {
                case 0: eliminator = c1Cds[1]; break;
                case 1: eliminator = c1Cds[0]; break;
            }

            for (int x = 0; x < counter; x++)
            {
                if ((GetBlock(i2, j2) == GetBlock(ctnr[x, 0], ctnr[x, 1])) && ctnr[x, 0] != i1 && ctnr[x, 1] != j1)
                {
                    if (candidates[ctnr[x, 0], ctnr[x, 1], val2] == 0 && candidates[ctnr[x, 0], ctnr[x, 1], eliminator] == 0)
                    {
                        //getTb(ctnr[x, 0], ctnr[x, 1]).BackColor = Color.Thistle;
                        if (i1 == i2)
                        {
                            for (int j = 1; j < 10; j++)
                            {
                                if (candidates[ctnr[x, 0], j, eliminator] == 0 && GetBlock(ctnr[x, 0], j) == GetBlock(i1, j1))
                                {
                                    //getTb(ctnr[x, 0], j).BackColor = Color.Black;
                                    methodNo[6]++;
                                    RemoveCandidate(ctnr[x, 0], j, eliminator);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                if (candidates[i, ctnr[x, 1], eliminator] == 0 && GetBlock(i, ctnr[x, 1]) == GetBlock(i1, j1))
                                {
                                    //getTb(i, ctnr[x, 1]).BackColor = Color.Black;
                                    methodNo[6]++;
                                    RemoveCandidate(i, ctnr[x, 1], eliminator);
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
            sbyte[,,] oldCandidates = new sbyte[10, 10, 10];
            int[,] oldArr = new int[10, 10];
            for (int k = 1; k < 10; k++)
            {
                for (int l = 1; l < 10 && i == 0; l++)
                {
                    if (candidates[k, l, 0] == 2)
                    {
                        i = k;
                        j = l;
                    }
                }
            }

            if (i != 0)
            {
                int[] last2 = candidates.ToArray(i, j);
                int filledOld = filled;
                Array.Copy(arr, oldArr, arr.Length);
                Array.Copy(candidates, oldCandidates, candidates.Length);
                dp = dp + "\n(" + i.ToString() + j.ToString() + " - " + last2[0].ToString() + ") ";
                Fill(i, j, last2[0]);

                bool proceed = TryToSolve();

                if (filled < 81 || proceed)
                {
                    Restore(oldArr, oldCandidates, filledOld);
                    dp = dp + "\n(" + i.ToString() + j.ToString() + " - " + last2[1].ToString() + ") ";
                    Fill(i, j, last2[1]);
                    proceed = TryToSolve();
                }

                if (filled < 81 || proceed)
                {
                    Restore(oldArr, oldCandidates, filledOld);
                }
            }
            depth--;
        }

        void Restore(int[,] oldArr, sbyte[,,] oldCandidates, int filledOld)
        {
            filled = filledOld;
            error = false;
            Array.Copy(oldArr, arr, arr.Length);
            Array.Copy(oldCandidates, candidates, candidates.Length);
        }
    }
}
