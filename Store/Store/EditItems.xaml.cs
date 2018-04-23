using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;

namespace Store
{
    /// <summary>
    /// Interaction logic for EditItems.xaml
    /// </summary>
    public partial class EditItems : Window
    {
        //целта на mainWindow e да може той да се използва в child windows
        //друг вариант е последния коментар на
        //https:/stackoverflow.com/questions/1130208/how-to-disable-the-parent-form-when-a-child-form-is-active
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow; 

        public class ShopItem
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        public class ShopItems : ObservableCollection<ShopItem>
        {

        }

        public ShopItems AllShopItems { get; } = new ShopItems();

        //защо трябва да се деклариреа клас който да имплементира ObservableCollection
        //вместо да се направи инстанция на ObservableCollection ?
        //ObservableCollection<ShopItem> SssItems { get; } = new ObservableCollection<ShopItem>();

        public EditItems()
        {
            //необходимо за listBox Binding
            //на какво е равно dataContext before that
            this.DataContext = this;
            mainWindow.IsEnabled = false;
            LoadShopItems();
            InitializeComponent();
            //сортиране на Елементите в itemsBox (индексът на елементите също се променя)
            itemsBox.Items.SortDescriptions.Add(
                 new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void LoadShopItems()
        {
            //прочита се файла с продуктите
            var xItems = from i in MainWindow.xdoc.Descendants("item")
                           select new
                           {
                               name = i.Attribute("name").Value,
                               price = i.Attribute("price").Value
                           };
            //добавят се продуктите в AllShopItems
            foreach (var i in xItems)
            {
                ShopItem str = new ShopItem()
                {
                    Name = i.name,
                    Price = i.price
                };
                AllShopItems.Add(str);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (itemsBox.SelectedIndex != -1)
            {
            }
        }

        private void DeleteButton_click(object sender, RoutedEventArgs e)
        {
            var xmlItems = from i in MainWindow.xdoc.Descendants("item")
                           select new
                           {
                               name = i.Attribute("name").Value,
                               price = i.Attribute("price").Value
                           };
            if (itemsBox.SelectedIndex != -1)
            {
                MessageBox.Show(((ShopItem)itemsBox.SelectedItem).Name.ToString());
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PriceTextBox_Click(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).IsReadOnly = false;
            ((TextBox)sender).SelectAll();
            ((TextBox)sender).Background = Brushes.White;
            //((TextBox)sender).BorderThickness = new Thickness() { Top = 1, Bottom = 1, Left = 1, Right = 1 };
        }

        private void PriceTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Background = Brushes.White;
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }

        private void PriceTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Background = Brushes.Transparent;
            ((TextBox)sender).BorderBrush = Brushes.Transparent;
        }
    }
}
