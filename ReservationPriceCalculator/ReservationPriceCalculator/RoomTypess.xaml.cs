using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for RoomTypess.xaml
    /// </summary>
    public partial class RoomTypes : Window
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private List<Room> RoomsList { get; } = new List<Room>();
        IEnumerable<XElement> xRooms;
        string oldValue;
        public static XDocument xDoc;

        private struct Room
        {
            public string Name { get; set; }
            public string Out { get; set; }
            public string Low { get; set; }
            public string High { get; set; }

            public Room(string n, string o, string l, string h)
            {
                this.Name = n;
                this.Out = o;
                this.Low = l;
                this.High = h;
            }
        }

        public RoomTypes()
        {
            this.DataContext = this;
            xDoc = XDocument.Load(App.settingsXml);
            LoadData();
            InitializeComponent();
            lbRoomTypes.ItemsSource = this.RoomsList;
        }

        private void LoadData()
        {
            xRooms = from item in xDoc.Descendants("Room")
                     select item;
            foreach (var room in xRooms)
            {
                Room r = new Room(room.Value, room.Attribute("out").Value, room.Attribute("low").Value, room.Attribute("high").Value);
                RoomsList.Add(r);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            xDoc.Save(App.settingsXml);
        }

        private void TextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {

            oldValue = ((TextBox)sender).Text;
            lbRoomTypes.SelectedIndex = -1;
            GetListViewFromChild((DependencyObject)sender).IsSelected = true;
        }

        private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if(((TextBox)sender).Text != oldValue)
            {
                DependencyObject stackPanel = VisualTreeHelper.GetParent((DependencyObject)sender);
                string[] textBoxText = new string[4];

                for (int i = 0; i < 4; i++)
                {
                    textBoxText[i] = ((TextBox)VisualTreeHelper.GetChild(stackPanel, i)).Text;
                }

                foreach (XElement room in xRooms)
                {
                    if (room.Value == textBoxText[0] || room.Value == oldValue)
                    {
                        tester.Text = room.Value;
                        room.Value = textBoxText[0];
                        room.Attribute("out").Value = textBoxText[1];
                        room.Attribute("low").Value = textBoxText[2];
                        room.Attribute("high").Value = textBoxText[3];
                    }
                }
            }
        }

        //Retreives the parent ListBoxItem (GOOD RECURSION METHOD)
        private ListBoxItem GetListViewFromChild(DependencyObject obj)
        {
            if (obj == null) return null;
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            if (parent == null) return null;
            if (parent is ListBoxItem) return parent as ListBoxItem;
            return GetListViewFromChild(parent);
        }
    }
}