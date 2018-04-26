using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace TotalAmount
{
    public class Reservation : ObservableCollection<Item>
    {
        public decimal Total { get; set; }

        public void AddToReservation(Item itm)
        {
            bool newItem = true;
            foreach (Item i in this)
            {
                if (i.Name == itm.Name && DateTime.Compare(i.CheckInDate, itm.CheckInDate) == 0 && DateTime.Compare(i.CheckOutDate, itm.CheckOutDate) == 0)
                {
                    i.Quantity += itm.Quantity;
                    newItem = false;
                }
            }
            if (newItem) this.Add(itm);
            TotalRefresh();

        }

        //This method also looks after the empty pirce columns and hides them
        public void TotalRefresh()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            this.Total = 0;
            bool[] priceIsZero = { true, true, true};
            foreach (Item i in this)
            {
                this.Total += i.Price[3];
                if (i.Days[0] != 0) priceIsZero[0] = false;
                if (i.Days[1] != 0) priceIsZero[1] = false;
                if (i.Days[2] != 0) priceIsZero[2] = false;
            }
            mainWindow.TbTotal.Text = this.Total.ToString();
            mainWindow.HideZeroValues(priceIsZero);
        }
    }
}
