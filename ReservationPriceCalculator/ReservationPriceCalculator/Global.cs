using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TotalAmount
{
    public static class Global
    {
        public static void KeyPressed(object sender)
        {
            MessageBox.Show(((TextBlock)sender).Text);
                //how to make the tester control public
                // to show the pressed key
        }
    }
}
