﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static XDocument xDoc = XDocument.Load(App.settingsXml);
        public static Dictionary<string, decimal[]> rooms = new Dictionary<string, decimal[]>();
        public Reservation Reservation { get; } = new Reservation();

        public int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8 };
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadRoomTypes();
            cbQuantity.ItemsSource = arr.ToArray();
            #region
            //can set ItemSource directly to rooms.Keys since it returns a collection
            //List<string> keyList = new List<string>(rooms.Keys);
            cbName.ItemsSource = rooms.Keys;
            #endregion
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

        //Loading room types and prices from the file into rooms dictionary
        private void LoadRoomTypes()
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
                decimal.TryParse(i._out, out arr[0]);
                decimal.TryParse(i.low, out arr[1]);
                decimal.TryParse(i.high, out arr[2]);
                rooms.Add(i.name.ToString(), arr);
            }
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
    }
}
