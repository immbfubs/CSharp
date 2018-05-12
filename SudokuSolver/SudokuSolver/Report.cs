using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Report : Form
    {
        public Report(string[] listItems)
        {
            InitializeComponent();
            listView1.View = View.Details;
            //-2 indicates Auto-Size
            listView1.Columns.Add("Substitution history", -2);
            foreach (string s in listItems)
            {
                listView1.Items.Add(new ListViewItem(s));
            }
        }
    }
}
