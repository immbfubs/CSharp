using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Globalization;

namespace Store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ItemsCollection Basket { get; } = new ItemsCollection();
        public static XDocument xdoc = XDocument.Load(Application.itemsXmlPath);
        public static Dictionary<string, string> dict = new Dictionary<string, string>();

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            LoadItems(true);
        }

        private void LoadItems(bool firstLoad)
        {
            if (firstLoad)
            {
                //--ADD ITEMS FROM xdoc TO dict
                var shopItems = from i in xdoc.Descendants("item")
                                select new
                                {
                                    name = i.Attribute("name").Value,
                                    price = i.Attribute("price").Value
                                };
                foreach (var i in shopItems)
                {
                    dict.Add(i.name, i.price);
                }
                //--END.
            }
            var sItems = from i in xdoc.Descendants("item")
                         select i.Attribute("name").Value;
            //мога ли да използвам shopItems.name вместо sItems
            cbName.ItemsSource = sItems.ToList();
        }

        private void Add()
        {
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            CultureInfo provider = CultureInfo.InvariantCulture;
            if (Decimal.TryParse(tbPrice.Text, style, provider, out decimal price) && Decimal.TryParse(tbQuantity.Text, style, provider, out decimal quantity))
            {
                Item itm = new Item
                {
                    Name = cbName.Text,
                    Price = price,
                    Quantity = quantity,
                    TotalPrice = Decimal.Round(quantity * price, 2)
                };
                Basket.PutInBasket(itm);
                TotalRefresh();
                lBox.SelectedIndex = lBox.Items.Count - 1;
                lBox.ScrollIntoView(lBox.SelectedItem);
                //Как мога да преместя последните два реда в MyCollection.cs ?
                //cbName.SelectedItem = null;
                cbName.Focus();
                tbQuantity.Text = "";
            }
            else
            {
                MessageBox.Show("Decimal.TryParse unsuccessful");
            }
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) Add();
        }

        private void TotalRefresh()
        {
            Basket.Total = 0;
            foreach (Item i in Basket)
            {
                Basket.Total += i.TotalPrice;
            }
            TbTotal.Text = Basket.Total.ToString();
        }

        private void CbName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tbPrice.Text = dict[e.AddedItems[0].ToString()].ToString();
        }

        void ChildWindow_Closed(object sender, EventArgs e)
        {
            LoadItems(false);
            this.IsEnabled = true;
        }

        #region Click Events

        private void Add_Click(object sender, RoutedEventArgs e) => Add();

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((Item)lBox.Items.GetItemAt(lBox.SelectedIndex)).Name.ToString());
            if (lBox.SelectedIndex != -1)
            {
                Basket.RemoveAt(lBox.SelectedIndex);
                TotalRefresh();
            }
        }

        private void BtnEditItems_Click(object sender, RoutedEventArgs e)
        {
            EditItems editItemsWindow = new EditItems();
            editItemsWindow.Show();
            editItemsWindow.Closed += new EventHandler(ChildWindow_Closed);
            //Ако използвам ShowDialog(), editItemsWindow.Closed няма да се регистрира от EventHandler ?
            //За да се деактивира този прозорец се използва this.Owner.IsEnabled = false; в отворените от него прозорци
        }

        private void BtnEditItems2_Click(object sender, RoutedEventArgs e)
        {
            EditItems2 editItems2Window = new EditItems2();
            editItems2Window.Show();
            editItems2Window.Closed += new EventHandler(ChildWindow_Closed);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Изтрийте я и си инсталирайте тая на Минчев!");
        }
        #endregion
    }
}
