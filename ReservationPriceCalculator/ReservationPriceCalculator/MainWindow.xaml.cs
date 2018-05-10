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
using System.Windows.Input;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static XDocument xDoc;
        public struct Room
        {
            public byte Guests { get; set; }
            public decimal Out { get; set; }
            public decimal Low { get; set; }
            public decimal High { get; set; }

            public Room(byte g, decimal o, decimal l, decimal h)
            {
                this.Guests = g;
                this.Out = o;
                this.Low = l;
                this.High = h;
            }
        }
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        public enum Reason
        {
            newFile,
            syntaxError,
            pricesError,
            datesError,
            notify
        }
        public enum Setting {
            addBedSmallPrice,
            addBedBigPrice
            }

        //Lentgth of Settings should be bigger than Setting biggest value
        public int[] Settings = new int[5];
        public struct Seasson
        {
            public int day;
            public int month;
            public string name;
        }
        public Seasson[] seassons = new Seasson[5];
        public Reservation Reservation { get; } = new Reservation();

        /// <summary>
        /// binding array for rooms quantity ComboBox
        /// </summary>
        public int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public MainWindow()
        {
            LoadSettingsFile();
            InitializeComponent();
            DataContext = this;
            LoadRoomTypes();
            LoadDates();
            LoadSettings();
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
                ResetSettings(Reason.newFile);
            }
            else
            {
                try
                {
                    xDoc = XDocument.Load(App.settingsXml);
                }
                catch
                {
                    ResetSettings(Reason.syntaxError);
                }
            }
        }

        /// <summary>Loading room types and prices from the file into rooms dictionary.</summary>
        /// <returns>Returns true if the settings file has been reset</returns>
        public bool LoadRoomTypes()
        {
            bool reload, fileReset = false;
            do
            {
                reload = false;
                try
                {
                    var types = from i in xDoc.Descendants("Room")
                                select new
                                {
                                    name = i.Value,
                                    guests = i.Attribute("guests").Value,
                                    _out = i.Attribute("out").Value,
                                    low = i.Attribute("low").Value,
                                    high = i.Attribute("high").Value
                                };
                    rooms.Clear();
                    foreach (var i in types)
                    {
                        Room r = new Room(
                            byte.Parse(i.guests),
                            decimal.Parse(i._out),
                            decimal.Parse(i.low),
                            decimal.Parse(i.high));
                        rooms.Add(i.name.ToString(), r);
                    }
                }
                catch (Exception e)
                {
                    ResetSettings(Reason.pricesError);
                    reload = fileReset = true;
                }
            } while (reload);
            return fileReset;
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
                    ResetSettings(Reason.datesError);
                    reload = true;
                }
            } while (reload);
        }

        //Loading program settings
        public void LoadSettings()
        {
            bool reload;
            do
            {
                reload = false;
                try
                {
                    var xDocSettings = from item in xDoc.Descendants("Setting")
                                       select new
                                       {
                                           name = item.Value,
                                           option = item.Attribute("option").Value
                                       };
                    foreach (var i in xDocSettings)
                    {
                        Settings[int.Parse(i.name)] = int.Parse(i.option);
                    }
                }
                catch
                {
                    ResetSettings(Reason.syntaxError);
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

        private void MenuClick_Settings(object sender, RoutedEventArgs e)
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
            if (message == MessageBoxResult.Yes) ResetSettings(Reason.notify);
        }

        private void ResetSettings(Reason r)
        {
            try
            {
                //string path = @"settings.xml";
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}
                
                xDoc = new XDocument(
                            new XDeclaration("1.0", "utf-16", "true"),
                            new XElement("Settings",
                                new XComment("Values must be decimals since decimal.TryParse is used!The rooms count can vary"),
                                new XElement("Room", "DBL", new XAttribute("guests", 2), new XAttribute("low", 99), new XAttribute("high", "119"), new XAttribute("out", "59")),
                                new XElement("Room", "DBLSV", new XAttribute("guests", 2), new XAttribute("low", 119), new XAttribute("high", "149"), new XAttribute("out", "79")),
                                new XElement("Room", "DBLsSV", new XAttribute("guests", 2), new XAttribute("low", 109), new XAttribute("high", "139"), new XAttribute("out", "69")),
                                new XElement("Room", "---------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
                                new XElement("Room", "SNGL", new XAttribute("guests", 1), new XAttribute("low", 79), new XAttribute("high", "99"), new XAttribute("out", "49")),
                                new XElement("Room", "SNGLSV", new XAttribute("guests", 1), new XAttribute("low", 99), new XAttribute("high", "119"), new XAttribute("out", "69")),
                                new XElement("Room", "SNGLsSV", new XAttribute("guests", 1), new XAttribute("low", 89), new XAttribute("high", "109"), new XAttribute("out", "59")),
                                new XElement("Room", "--------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
                                new XElement("Room", "TRPL", new XAttribute("guests", 3), new XAttribute("low", 149), new XAttribute("high", "189"), new XAttribute("out", "99")),
                                new XElement("Room", "-------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
                                new XElement("Room", "AP3", new XAttribute("guests", 4), new XAttribute("low", 199), new XAttribute("high", "249"), new XAttribute("out", "159")),
                                new XElement("Room", "AP4", new XAttribute("guests", 4), new XAttribute("low", 199), new XAttribute("high", "249"), new XAttribute("out", "159")),
                                new XElement("Room", "PresAP", new XAttribute("guests", 4), new XAttribute("low", 249), new XAttribute("high", "299"), new XAttribute("out", "239")),
                                new XComment("Values must be integers since int.Parse is used!Exactly 4 dates must be defined!"),
                                new XElement("Date", "Силен летен", new XAttribute("day", 1), new XAttribute("month", 7)),
                                new XElement("Date", "Извън есзона", new XAttribute("day", 1), new XAttribute("month", 10)),
                                new XElement("Date", "Слаб летен - есен", new XAttribute("day", 16), new XAttribute("month", 9)),
                                new XElement("Date", "Слаб летен - пролет", new XAttribute("day", 25), new XAttribute("month", 5)),
                                new XElement("Setting", (int)Setting.addBedBigPrice, new XAttribute("option", 35)),
                                new XElement("Setting", (int)Setting.addBedSmallPrice, new XAttribute("option", 30))));
                xDoc.Save(App.settingsXml);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            switch (r)
            {
                case Reason.newFile:
                    MessageBox.Show("Нов файлй с настройки беше създаден.");
                    break;
                case Reason.syntaxError:
                    MessageBox.Show("Файлът с настройки беше рестартиран поради грешка в текста.");
                    break;
                case Reason.pricesError:
                    MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от ценните не бяха зададена коректно.");
                    break;
                case Reason.datesError:
                    MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от началните дати на сезоните не бяха зададени коректно.");
                    break;
                default:
                    MessageBox.Show("Файлът с настройките беше рестартиран.\nВсички дати, типове стаи и цени бяха рестартирани!");
                    break;
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.ClearFocus();
            }
        }
    }
}
