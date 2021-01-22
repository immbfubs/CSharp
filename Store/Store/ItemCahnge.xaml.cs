using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace Store
{
    /// <summary>
    /// Interaction logic for ItemCahnge.xaml
    /// </summary>
    public partial class ItemCahnge : Window
    {
        public ItemCahnge(ref KeyValuePair<string, string> oldItem)
        {
            InitializeComponent();
            nameTB.Text = oldItem.Key;
            priceTB.Text = oldItem.Value;
        }

        private void SaveButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            CultureInfo provider = CultureInfo.InvariantCulture;
            if (Decimal.TryParse(priceTB.Text, style, provider, out decimal result))
            {
                MainWindow.dict[nameTB.Text] = priceTB.Text;
                var element = MainWindow.xdoc.Descendants().Elements("item").SingleOrDefault(x => x.Attribute("name").Value == nameTB.Text);
                element.SetAttributeValue("price", priceTB.Text);
                MainWindow.xdoc.Save(Application.itemsXmlPath);
                this.Close();
            }
            else
            {
                MessageBox.Show("Въведете валидна стойност за цена!");
            }
        }

        private void CancelButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
