using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOTO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[] arr = new int[6];
        int[] drawn = new int[6];
        int counter = 0;

        private void button_Click(object sender, EventArgs e)
        {
            if (counter < 6)
            {
                Button btn = (Button)sender;
                btn.Enabled = false;
                btn.BackColor = System.Drawing.Color.White;
                arr[counter] = int.Parse(btn.Text);
                counter++;
                if (counter == 6)
                {
                    counter = 0;
                    check();
                }
            }
        }

        private void check()
        {
            Random rand = new Random();
            int draw, congruences = 0;
            bool notDrown;
            do
            {
                draw = rand.Next(1, 50);
                notDrown = true;
                foreach (int a in drawn)
                {
                    if (a == draw)
                    {
                        notDrown = false;
                    }
                }
                if (notDrown)
                {
                    drawn[counter] = draw;
                    counter++;
                    string name = "button" + draw.ToString();
                    object[] buttons = this.Controls.Find(name, true);
                    Button btn = (Button)buttons[0];
                    btn.ForeColor = System.Drawing.Color.Red;
                    foreach (int a in arr)
                    {
                        if (a == draw)
                        {
                            congruences++;
                            btn.BackColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            while (counter < 6);
            label.Text = congruences.ToString()+" Matches";
        }

        private void button50_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Application.ExecutablePath.ToString());
            System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
            this.Close(); //to turn off current app
        }
    }
}
