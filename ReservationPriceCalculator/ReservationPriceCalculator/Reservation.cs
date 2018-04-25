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

        public void TotalRefresh()
        {
            this.Total = 0;
            foreach (Item i in this)
            {
                this.Total += i.Price[3];
            }
            ((MainWindow)Application.Current.MainWindow).TbTotal.Text = this.Total.ToString();
        }
    }
}
