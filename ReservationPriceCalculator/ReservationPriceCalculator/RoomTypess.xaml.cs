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

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for RoomTypess.xaml
    /// </summary>
    public partial class RoomTypess : Window
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        struct Room
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

        ObservableCollection<Room> RoomsList { get; } = new ObservableCollection<Room>();

        public RoomTypess()
        {
            this.DataContext = this;
            LoadData();
            InitializeComponent();
            lbRoomTypes.ItemsSource = this.RoomsList;
        }

        private void LoadData()
        {
            var xRooms = from room in MainWindow.xDoc.Descendants("Room")
                        select new
                        {
                            name = room.Value,
                            Out = room.Attribute("out").Value,
                            low = room.Attribute("low").Value,
                            high = room.Attribute("high").Value
                        };

            foreach (var room in xRooms)
            {
                Room r = new Room( room.name, room.Out, room.low, room.high);
                RoomsList.Add(r);
            }
        }
    }
}
