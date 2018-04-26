﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace TotalAmount
{
    public class Item : INotifyPropertyChanged
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public string Name { get; set; }
        private int quantity;
        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                bool refreshPrice = true;
                if(quantity == 0)
                {
                    refreshPrice = false;
                }
                quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                if (refreshPrice) this.CalculatePrice();
            }
        }
        private decimal[] price = new decimal[4];
        public decimal[] Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
            }
        }
        public int[] Days { get; set; } = new int[4];
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Item(string n, int q, DateTime cin, DateTime cout)
        {
            Name = n;
            Quantity = q;
            CheckInDate = cin;
            CheckOutDate = cout;
            CalculatePrice();
        }
        
        public void CalculatePrice()
        {
            int a,b;
            //if Price[i] gets addressed PropertyChanged is not working. Why? Setter is not invoked that way?
            decimal[] arri = new decimal[4];
            DateTime date0 = new DateTime(CheckInDate.Year, mainWindow.dates[0, 0], mainWindow.dates[0, 1]);
            DateTime date1 = new DateTime(CheckInDate.Year, mainWindow.dates[1, 0], mainWindow.dates[1, 1]);
            DateTime date2 = new DateTime(CheckInDate.Year, mainWindow.dates[2, 0], mainWindow.dates[2, 1]);
            DateTime date3 = new DateTime(CheckInDate.Year, mainWindow.dates[3, 0], mainWindow.dates[3, 1]);

            MessageBox.Show(date0.ToString());
            MessageBox.Show(date1.ToString());
            MessageBox.Show(date2.ToString());
            MessageBox.Show(date3.ToString());

            //Determine in which tourist seassons are Check-In and Check-Out dates
            if (CheckInDate.CompareTo(date0) < 0) a = 0;
            else if (CheckInDate.CompareTo(date1) < 0) a = 1;
            else if (CheckInDate.CompareTo(date2) < 0) a = 2;
            else if (CheckInDate.CompareTo(date3) < 0) a = 3;
            else a = 0;
            if (CheckOutDate.CompareTo(date0) < 0) b = 0;
            else if (CheckOutDate.CompareTo(date1) < 0) b = 1;
            else if (CheckOutDate.CompareTo(date2) < 0) b = 2;
            else if (CheckOutDate.CompareTo(date3) < 0) b = 3;
            else b = 0;

            if(a == b)
            {
                if (a == 3) a = 1;
                Days[a] = (int)(Math.Round((CheckOutDate - CheckInDate).TotalHours) / 24);
                arri[a] = Days[a] * MainWindow.rooms[this.Name][a];
            }
            else if(a == 0 && b == 1)
            {
                Days[0] = (int)(Math.Round((date0 - CheckInDate).TotalHours) / 24);
                arri[0] = Days[0] * MainWindow.rooms[this.Name][0];
                Days[1] = (int)(Math.Round((CheckOutDate - date0).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
            }
            else if (a == 1 && b == 2)
            {
                Days[1] = (int)(Math.Round((date1 - CheckInDate).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
                Days[2] = (int)(Math.Round((CheckOutDate - date1).TotalHours) / 24);
                arri[2] = Days[2] * MainWindow.rooms[this.Name][2];
            }
            else if (a == 2 && b == 3)
            {
                Days[2] = (int)(Math.Round((date2 - CheckInDate).TotalHours) / 24);
                arri[2] = Days[2] * MainWindow.rooms[this.Name][2];
                Days[1] = (int)(Math.Round((CheckOutDate - date2).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
            }
            else if (a == 3 && b == 0)
            {
                Days[1] = (int)(Math.Round((date3 - CheckInDate).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
                Days[0] = (int)(Math.Round((CheckOutDate - date3).TotalHours) / 24);
                arri[0] = Days[0] * MainWindow.rooms[this.Name][0];
            }
            else if (a == 0 && b == 2)
            {
                Days[0] = (int)(Math.Round((CheckInDate - date0).TotalHours) / 24);
                arri[0] = Days[0] * MainWindow.rooms[this.Name][0];
                Days[1] = (int)(Math.Round((date1 - date0).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
                Days[2] = (int)(Math.Round((CheckOutDate - date1).TotalHours) / 24);
                arri[2] = Days[2] * MainWindow.rooms[this.Name][2];
            }
            else if (a == 1 && b == 3)
            {
                Days[1] = (int)(Math.Round((date1 - CheckInDate).TotalHours) / 24);
                Days[2] = (int)(Math.Round((date2 - date1).TotalHours) / 24);
                arri[2] = Days[2] * MainWindow.rooms[this.Name][2];
                Days[1] += (int)(Math.Round((CheckOutDate - date2).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
            }
            else if (a == 2 && b == 0)
            {
                Days[2] = (int)(Math.Round((date2 - CheckInDate).TotalHours) / 24);
                arri[2] = Days[2] * MainWindow.rooms[this.Name][2];
                Days[1] = (int)(Math.Round((date3 - date2).TotalHours) / 24);
                arri[1] = Days[1] * MainWindow.rooms[this.Name][1];
                Days[0] = (int)(Math.Round((CheckOutDate - date3).TotalHours) / 24);
                arri[0] = Days[0] * MainWindow.rooms[this.Name][0];
            }
            arri[0] *= this.quantity;
            arri[1] *= this.quantity;
            arri[2] *= this.quantity;
            arri[3] = arri[0] + arri[1] + arri[2];
            Days[3] = Days[0] + Days[1] + Days[2];
            Price = arri;
        }
    }
}
