using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeaChess
{
    public class Chess
    {
        public bool humanBegins, humanWon, AIWon;
        public int[] b;
        bool[] bx, bo;
        public int filled, AImove, move;
        Random random;

        public Chess(bool whoBegins)
        {
            humanBegins = whoBegins;
            filled = 0;
            bx = new bool[10];
            bo = new bool[10];
            b = new int[10];
            random = new Random();
            humanWon = false;
            AIWon = false;
            move = 0;
        }

        public void Human(int index)
        {
            FillBox(index, 1);
            if (filled < 9)
            {
                AImove = AI();
            }
            else
            {
                //just to check if the Player has won
                CrucialMove(out int a);
            }
        }

        public int AI()
        {
            move++;
            //if First Move
            if (move == 1)
            {   
                return FirstMove();
            }

            int index;

            if (move == 2)
            {
                if (SecondMove(out index))
                {
                    FillBox(index, 2);
                    return index;
                }
            }
            
            if (CrucialMove(out index))
            {
                FillBox(index, 2);
                return index;
            }

            index = FindNextMove();
            FillBox(index, 2);
            return index;
        }

        private int FirstMove()
        {
            if (b[5] == 1)
            {
                //case Player beigins
                int index = Random(1,3,7,9);
                FillBox(index, 2);
                return index;
            }
            else
            {
                //case AI begins
                FillBox(5, 2);
                return 5;
            }
        }

        private bool SecondMove(out int index)
        {
            if (humanBegins)
            {
                // case 1
                if (bx[5])
                {
                    if ((b[1] + b[9]) == 3)
                    {
                        index = Random(3, 7);
                        return true;
                    }
                    if ((b[3] + b[7]) == 3)
                    {
                        index = Random(1, 9);
                        return true;
                    }
                }

                // case 2
                if ((bx[1] && bx[9]) || (bx[3] && bx[7]))
                {
                    index = Random(2, 4, 6, 8);
                    return true;
                }
            }

            if (!humanBegins)
            {
                // case 1
                if (bo[5])
                {
                    if (bx[1])
                    {
                        index = 9;
                    }
                    else if (bx[3])
                    {
                        index = 7;
                    }
                    else if (bx[7])
                    {
                        index = 3;
                    }
                    else if (bx[9])
                    {
                        index = 1;
                    }
                    else
                    {
                        index = Random(1, 3, 7, 9);
                    }
                    return true;
                }
            }

            // case "No Case"
            index = 666;
            return false;
        }

        private bool CrucialMove(out int index)
        {
            index = 0;
            int prior = 0;
            if (Check(ref index, ref prior, 1, 4, 7))
            {
                return true;
            }
            if (Check(ref index, ref prior, 2, 5, 8))
            {
                return true;
            }
            if (Check(ref index, ref prior, 3, 6, 9))
            {
                return true;
            }
            if (Check(ref index, ref prior, 1, 2, 3))
            {
                return true;
            }
            if (Check(ref index, ref prior, 4, 5, 6))
            {
                return true;
            }
            if (Check(ref index, ref prior, 7, 8, 9))
            {
                return true;
            }
            if (Check(ref index, ref prior, 1, 5, 9))
            {
                return true;
            }
            if (Check(ref index, ref prior, 3, 5, 7))
            {
                return true;
            }

            if (index > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int FindNextMove()
        {
            int index;
            
            //Why bother?

            //Random if none found
            index = Random();
            return index;
        }

        private bool Check(ref int index, ref int prior, int a, int b, int c)
        {
            int e = 0, empty = 0;

            //count empty boxes
            if (this.b[a] == 0)
            {
                e++;
                empty = a;
            }
            if (this.b[b] == 0)
            {
                e++;
                empty = b;
            }
            if (this.b[c] == 0)
            {
                e++;
                empty = c;
            }

            //check if player wins
            if (e == 0 && ((this.b[a] + this.b[b] + this.b[c]) == 3))
            {
                index = 0;
                humanWon = true;
                AIWon = false;
                return true;
            }
            //check for WIN move
            if (e == 1 && ((this.b[a] + this.b[b] + this.b[c]) == 4))
            {
                prior = 1;
                AIWon = true;
                index = empty;
            }
            
            //check for two 1s
            if (e == 1 && ((this.b[a] + this.b[b] + this.b[c]) == 2) && prior == 0)
            {
                index = empty;
            }
            return false;
        }

        private void FillBox(int index, int value)
        {
            //ERROR check
            if (b[index] > 0)
            {
                MessageBox.Show("FillBox Error 1 \n\nbox " + index.ToString());
            }

            //Fill box[]
            b[index] = value;
            filled++;

            //bx, bo
            if(value == 1)
            {
                bx[index] = true;
            }
            if(value == 2)
            {
                bo[index] = true;
            }
        }
        
        private int Random(params int[] values)
        {
            // if values[] is empty, fill it with all empty boxes' indexes
            if (values.Length == 0)
            {
                Array.Resize(ref values, (9 - filled));
                int j = 0;
                for (int i = 1; i < 10; i++)
                {
                    if (b[i] == 0)
                    {
                        values[j++] = i;
                    }
                }
            }

            // get a random number from values[]
            int index;
            index = random.Next(0, values.Length);
            return values[index];
        }

        public string ArrayToString()
        {
            string str = "";
            for (int i = 1; i < 10; i++)
            {
                str = str + b[i].ToString() + "   ";
                if (i == 3 || i == 6 || i == 9)
                {
                    str = str + "\n";
                }
            }
            return str;
        }
    }
}