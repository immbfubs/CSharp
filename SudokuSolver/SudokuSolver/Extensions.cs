using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    static class Extensions
    {
        public static string ToString(this sbyte[,,] candidates, int i, int j)
        {
            StringBuilder sb = new StringBuilder();
            for (int n = 1; n < 10; n++)
            {
                if (candidates[i, j, n] == 0) sb.Append(n + " ");
            }
            return sb.ToString();
        }

        public static int Last(this sbyte[,,] candidates, int i, int j)
        {
            for (int n = 1; n < 10; n++)
            {
                if (candidates[i, j, n] == 0) return n;
            }
            return -2;
        }

        public static int[] ToArray(this sbyte[,,] candidates, int i, int j)
        {
            int[] theArray = new int[candidates[i, j, 0]];
            int k = 0;
            for (int n = 1; n < 10; n++)
            {
                if (candidates[i, j, n] == 0)
                {
                    theArray[k] = n;
                    k++;
                }
            }
            return theArray;
        }

        public static bool AreEqual(this sbyte[,,] candidates, int i1, int j1, int i2, int j2)
        {
            for (int n = 0; n < 10; n++)
            {
                if (candidates[i1, j1, n] != candidates[i2, j2, n]) return false;
            }
            return true;
        }
    }
}
