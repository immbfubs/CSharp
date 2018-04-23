using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        int op, brac;
        double arg1, arg2, ans, m;
        bool clr, getArg2;
        string last;
        double[,] arg = new double[8,2];


        public Form1()
        {
            InitializeComponent();
            clearAll();
        }

        private void calculate()
        {
            if (!getArg2)
            {
                arg1 = Prs(text.Text);
                clr = true;
                return;
            }
            if (last == "cif" || last == "m")
            {
                arg2 = Prs(text.Text);
            }

            switch (op)
            {
                case 1:
                    ans = arg1 + arg2;
                    break;
                case 2:
                    ans = arg1 - arg2;
                    break;
                case 3:
                    ans = arg1 * arg2;
                    break;
                case 4:
                    ans = arg1 / arg2;
                    break;
            }
            text.Text = ans.ToString();
            arg1 = ans;
            clr = true;
        }

        private void clearAll()
        {
            clr = false;
            getArg2 = false;
            text.Clear();
            arg1 = 0;
            arg2 = 0;
        }

        private double Prs(string a)
        {
            double b;
            double.TryParse(a, out b);
            return b;
        }

        private void cipher_Click(object sender, EventArgs e)
        {
            if (last == "eq")
            {
                clearAll();
            }
            if (clr)
            {
                text.Clear();
                clr = false;
            }
            
            Button btn = (Button)sender;
            text.Text += btn.Text;
            last = "cif";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (last != "op" && last != "eq" && last != "open")
            {
                calculate();
            }
                getArg2 = true;
            last = "op";
            op = 1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (last != "op" && last != "eq" && last != "open")
            {
                calculate();
            }
                getArg2 = true;
            last = "op";
            op = 2;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (last != "op" && last != "eq" && last != "open")
            {
                calculate();
            }
                getArg2 = true;
            last = "op";
            op = 3;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (last != "op" && last != "eq" && last != "open")
            {
                calculate();
            }
                getArg2 = true;
            last = "op";
            op = 4;
        }

        private void equals_Click(object sender, EventArgs e)
        {
            if (brac > 0)
            {
                if (last == "op")
                {
                    MessageBox.Show("Bad input", "Error");
                    return;
                }
                DialogResult res = MessageBox.Show("Close all brackets?", "U sure?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    while (brac > 0)
                    {
                        calculate();
                        arg1 = arg[brac, 0];
                        op = Convert.ToInt32(arg[brac, 1]);
                        brac = brac - 1;
                        brackets.Text = brac.ToString();
                        getArg2 = true;
                    }
                brackets.Text = "";
                }
                else
                {
                    return;
                }
            }
            calculate();
            last = "eq";
        }

        private void del_Click(object sender, EventArgs e)
        {
            if (text.Text.Length > 0)
            {
                text.Text = text.Text.Substring(0, text.Text.Length - 1 );
                if (getArg2)
                {
                    arg2 = Prs(text.Text);
                }
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            clearAll();
            brac = 0;
            brackets.Text = "";
            last = "C";
        }

        private void plus_min_Click(object sender, EventArgs e)
        {
            double taker;
            taker = Prs(text.Text);
            taker = taker * (-1);
            if (last == "eq" || last == "op")
            {
                clearAll();
                arg1 = taker;
                getArg2 = true;
            }
            text.Text = taker.ToString();
            clr = true;
        }

        private void sqr_Click(object sender, EventArgs e)
        {
            double taker;
            taker = Prs(text.Text);
            taker = taker * taker;
            text.Text = taker.ToString();
            if (last != "cif" && last != "m")
            {
                arg1 = arg1 * arg1;
            }
        }

        private void root_Click(object sender, EventArgs e)
        {
            double taker;
            taker = Prs(text.Text);
            taker = Math.Sqrt(taker);
            text.Text = taker.ToString();
            if (last != "cif" && last != "m")
            {
                arg1 = Math.Sqrt(arg1);
            }
        }

        private void fact_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This button is fake", "Wait!", MessageBoxButtons.AbortRetryIgnore);
            //TO BE CONTINUED
        }

        private void m_save_Click(object sender, EventArgs e)
        {
            m = Prs(text.Text);
        }

        private void m_plus_Click(object sender, EventArgs e)
        {
            m = m + Prs(text.Text);
        }

        private void m_minus_Click(object sender, EventArgs e)
        {
            m = m - Prs(text.Text);
        }

        private void m_return_Click(object sender, EventArgs e)
        {
            text.Text = m.ToString();
            arg2 = m;
            last = "m";
        }

        private void m_clear_Click(object sender, EventArgs e)
        {
            m = 0;
        }

        private void brak_open_Click(object sender, EventArgs e)
        {
            if (last == "op")
            {
                if (brac == 6)
                {
                    MessageBox.Show("Ай стига толкова скоби, а?!");
                    return;
                }
                brac = brac + 1;
                arg[brac,0] = Prs(text.Text);
                arg[brac,1] = op;
                clearAll();
                brackets.Text = brac.ToString();
                
            }
            last = "open";
        }

        private void brak_close_Click(object sender, EventArgs e)
        {
            if (brac > 0)
            {
                calculate();
                arg1 = arg[brac,0];
                op = Convert.ToInt32(arg[brac,1]);
                brac = brac - 1;
                brackets.Text = brac.ToString();
                getArg2 = true;
                if (brac == 0)
                {
                    brackets.Text = "";
                }
            }
        }

        private void SHOW_VARS(object sender, EventArgs e)
        {
            MessageBox.Show(getArg2.ToString() + ", " + arg1.ToString() + ", "+ arg2 + ", "+op);
            //MessageBox.Show("Ако дойдеш да гледаме Старт Уарс, ще ти взема Бейлис");
        }
    }
}
