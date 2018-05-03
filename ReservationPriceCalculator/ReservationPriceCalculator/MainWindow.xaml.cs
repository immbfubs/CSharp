using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static XDocument xDoc;
        public static Dictionary<string, decimal[]> rooms = new Dictionary<string, decimal[]>();
        public struct Seasson
        {
            public int day;
            public int month;
            public string name;
        }
        public Seasson[] seassons = new Seasson[5];
        public Reservation Reservation { get; } = new Reservation();

        public int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8 };
        public MainWindow()
        {
            LoadSettingsFile();
            InitializeComponent();
            DataContext = this;
            LoadRoomTypes();
            LoadDates();
            cbQuantity.ItemsSource = arr.ToArray();
            #region
            //can set ItemSource directly to rooms.Keys since it returns a collection
            //List<string> keyList = new List<string>(rooms.Keys);
            cbName.ItemsSource = rooms.Keys;
            #endregion
        }

        //Try to load settings.xml
        public void LoadSettingsFile()
        {
            if (!File.Exists(App.settingsXml))
            {
                ResetSettings(0);
            }
            else
            {
                try
                {
                    xDoc = XDocument.Load(App.settingsXml);
                }
                catch
                {
                    ResetSettings(1);
                }
            }
        }

        //Loading room types and prices from the file into rooms dictionary
        public void LoadRoomTypes()
        {
            bool reload;
            do
            {
                reload = false;
                try
                {
                    var types = from i in xDoc.Descendants("Room")
                                select new
                                {
                                    name = i.Value,
                                    _out = i.Attribute("out").Value,
                                    low = i.Attribute("low").Value,
                                    high = i.Attribute("high").Value
                                };
                    foreach (var i in types)
                    {
                        decimal[] arr = new decimal[3];
                        arr[0] = decimal.Parse(i._out);
                        arr[1] = decimal.Parse(i.low);
                        arr[2] = decimal.Parse(i.high);
                        rooms.Add(i.name.ToString(), arr);
                    }
                }
                catch
                {
                    ResetSettings(2);
                    reload = true;
                }
            } while (reload);
        }

        //Loading the beginning dates of every seasson
        public void LoadDates()
        {
            bool reload;
            do
            {
                reload = false;
                try
                {
                    var xDocDates = from i in xDoc.Descendants("Date")
                                    select new
                                    {
                                        name = i.Value,
                                        day = int.Parse(i.Attribute("day").Value),
                                        month = int.Parse(i.Attribute("month").Value)
                                    };
                    var l = xDocDates.ToList();
                    l.Sort((x, y) => x.month.CompareTo(y.month));
                    int counter = 0;
                    foreach (var i in l)
                    {
                        seassons[counter].day = i.day;
                        seassons[counter].month = i.month;
                        seassons[counter].name = i.name;
                        counter++;
                    }
                }
                catch
                {
                    ResetSettings(3);
                    reload = true;
                }
            } while (reload);
        }

        //Hides zero columns 
        public void HideZeroValues(bool[] isZero)
        {
            Style hidden = new Style { TargetType = typeof(TextBlock) };
            Style visible = new Style { TargetType = typeof(TextBlock) };

            visible.Setters.Add(new Setter(TextBlock.VisibilityProperty, Visibility.Visible));
            hidden.Setters.Add(new Setter(TextBlock.VisibilityProperty, Visibility.Hidden));

            if (isZero[0]) this.Resources["Out"] = hidden;
                else this.Resources["Out"] = visible;
            if (isZero[1]) this.Resources["Low"] = hidden;
                else this.Resources["Low"] = visible;
            if (isZero[2]) this.Resources["High"] = hidden;
                else this.Resources["High"] = visible;
        }

        //btnAdd_Click invokes this method which checks if the selected period is valid
        private bool IsCorrectPeriodPicked()
        {
            if (dpCheckIn.SelectedDate != null && dpCheckOut.SelectedDate != null)
            {
                if (((DateTime)dpCheckIn.SelectedDate).CompareTo((DateTime)dpCheckOut.SelectedDate) < 0) return true;
            }
            MessageBox.Show("Датите нещо не ме кефят");
            return false;
        }

        private void dpCheckIn_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpCheckOut.SelectedDate == null)
            {
                dpCheckOut.SelectedDate = ((DateTime)dpCheckIn.SelectedDate).AddDays(1);
            }
            else if (((DateTime)dpCheckIn.SelectedDate).CompareTo((DateTime)dpCheckOut.SelectedDate) >= 0)
            {
                dpCheckOut.SelectedDate = ((DateTime)dpCheckIn.SelectedDate).AddDays(1);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsCorrectPeriodPicked())
            {
                Item itm = new Item(
                    cbName.SelectedValue.ToString(),
                    (int)cbQuantity.SelectedValue,
                    (DateTime)dpCheckIn.SelectedDate,
                    (DateTime)dpCheckOut.SelectedDate
                );
                Reservation.AddToReservation(itm);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(lBox.SelectedIndex != -1)
            {
                Reservation.RemoveAt(lBox.SelectedIndex);
                Reservation.TotalRefresh();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            int j = lBox.Items.Count;
            for (int i = 0; i < j; i++)
            {
                Reservation.RemoveAt(0);
            }
            Reservation.TotalRefresh();
        }

        private void MenuClick_Seassons(object sender, RoutedEventArgs e)
        {
            SeassonsWindow seassonsWindow = new SeassonsWindow();
            seassonsWindow.ShowDialog();
            LoadSettingsFile();
            LoadRoomTypes();
            LoadDates();
        }

        private void MenuClick_RoomTypes(object sender, RoutedEventArgs e)
        {
            RoomTypes roomsWindow = new RoomTypes();
            roomsWindow.ShowDialog();
        }

        private void MenuClick_ResetSettingsFile(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Всички сезони и цени ще бъдат рестартирани на първоначално зададените!\nСигурни ли сте?", "Внимание!", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes) ResetSettings(10);
        }

        private void ResetSettings(int sender)
        {
            string path = @"settings.xml";
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Settings>\n<!--Values must be decimals since decimal.TryParse is used!The rooms count can vary-->\n<Room low=\"99\" high=\"119\" out=\"59\">DBL</Room>\n<Room low=\"119\" high=\"149\" out=\"79\">DBLSV</Room>\n<Room low=\"109\" high=\"139\" out=\"69\">DBLsSV</Room>\n<Room low=\"0\" high=\"0\" out=\"0\">----</Room>\n<Room low=\"79\" high=\"99\" out=\"49\">SNGL</Room>\n<Room low=\"99\" high=\"119\" out=\"69\">SNGLSV</Room>\n<Room low=\"89\" high=\"109\" out=\"59\">SNGLsSV</Room>\n<Room low=\"0\" high=\"0\" out=\"0\">-----</Room>\n<Room low=\"149\" high=\"189\" out=\"99\">TRPL</Room>\n<Room low=\"0\" high=\"0\" out=\"0\">------</Room>\n<Room low=\"199\" high=\"249\" out=\"159\">AP3</Room>\n<Room low=\"199\" high=\"249\" out=\"159\">AP4</Room>\n<Room low=\"249\" high=\"299\" out=\"239\">PresAP</Room>\n<!--Values must be integers since int.Parse is used!Exactly 4 dates must be defined!-->\n<Date day=\"1\" month=\"7\">Силен летен</Date>\n<Date day=\"1\" month=\"10\">Извън есзона</Date>\n<Date day=\"16\" month=\"9\">Слаб летен - есен</Date>\n<Date day=\"25\" month=\"5\">Слаб летен - пролет</Date>\n</Settings>");
                    fs.Write(info, 0, info.Length);
                }
                xDoc = XDocument.Load(App.settingsXml);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            switch (sender)
            {
                case 0:
                    MessageBox.Show("Нов файлй с настройки беше създаден.");
                    break;
                case 1:
                    MessageBox.Show("Файлът с настройки беше рестартиран поради грешка в текста.");
                    break;
                case 2:
                    MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от ценните не бяха зададена коректно.");
                    break;
                case 3:
                    MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от началните дати на сезоните не бяха зададени коректно.");
                    break;
                default:
                    MessageBox.Show("Файлът с настройките беше рестартиран.\nВсички дати, типове стаи и цени бяха рестартирани!");
                    break;
            }
        }
    }
}
