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
using System.Collections.ObjectModel;

namespace Store
{
    /// <summary>
    /// Interaction logic for EditItems2.xaml
    /// </summary>
    public partial class EditItems2 : Window
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public EditItems2()
        {
            mainWindow.IsEnabled = false;
            Resources["Items"] = MainWindow.dict;
            InitializeComponent();
            itemsListView.Items.SortDescriptions.Add(
                 new System.ComponentModel.SortDescription("Key", System.ComponentModel.ListSortDirection.Ascending));
            itemsListView.Items.SortDescriptions.Add(
                 new System.ComponentModel.SortDescription("Key", System.ComponentModel.ListSortDirection.Descending));
            //Това добра необходимо ли е или ресурсите се изчистват сами? Кое ве лек?
            //MessageBox.Show(itemsListView.Items.SortDescriptions.Count.ToString());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemsListView.SelectedIndex != -1)
            {
                //Т'ва що не е дописано?
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemsListView.SelectedItems.Count == 1)
            {
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
                kvp = (KeyValuePair < string, string > )itemsListView.SelectedItem;
                ItemCahnge itemChangeWindow = new ItemCahnge(ref kvp);
                itemChangeWindow.Closed += new EventHandler(ItemsListViewLayoutUpdate);
                itemChangeWindow.Show();
            }
            else if (itemsListView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Моля изберете точно един продукт");
            }
            else
            {
                MessageBox.Show("Не сте избрали продукт!");
            }
        }

        void DeleteButton_click(object sender, RoutedEventArgs e)
        {
            if (itemsListView.SelectedIndex != -1)
            {
                foreach (KeyValuePair<string, string> kvp in itemsListView.SelectedItems)
                {
                    MessageBox.Show(kvp.Key.ToString());
                    var element = MainWindow.xdoc.Descendants().Elements("item").SingleOrDefault(x => x.Attribute("name").Value == kvp.Key);
                    element.Remove();
                    MainWindow.dict.Remove(kvp.Key);
                }
                MainWindow.xdoc.Save(Application.itemsXmlPath);
                ItemsListViewLayoutUpdate();
            }
        }

        void ItemsListViewLayoutUpdate(object sender, EventArgs e)
        {
            Resources.Clear();
            Resources["Items"] = MainWindow.dict;
            itemsListView.UpdateLayout();
        }

        void ItemsListViewLayoutUpdate()
        {
            Resources.Clear();
            Resources["Items"] = MainWindow.dict;
            itemsListView.UpdateLayout();
        }
    }
}
