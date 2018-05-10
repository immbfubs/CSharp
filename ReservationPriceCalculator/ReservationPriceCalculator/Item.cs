using System;
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
        private byte discount;
        public byte Discount {
            get { return discount; }
            set
            {
                discount = value;
                this.CalculatePrice();
                mainWindow.Reservation.TotalRefresh();
            }

        }
        private int quantity;
        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                this.CalculatePrice();
                mainWindow.Reservation.TotalRefresh();
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
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int[] Days { get; set; } = new int[4];
        //test for 255+
        private sbyte bedsSmall;
        public sbyte BedsSmall
        {
            get { return bedsSmall; }
            set
            {
                bedsSmall = value;
                this.CalculatePrice();
                mainWindow.Reservation.TotalRefresh();
            }

        }
        private sbyte bedsBig;
        public sbyte BedsBig
        {
            get { return bedsBig; }
            set
            {
                bedsBig = value;
                this.CalculatePrice();
                mainWindow.Reservation.TotalRefresh();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Item(string n, int q, DateTime cin, DateTime cout)
        {
            Name = n;
            CheckInDate = cin;
            CheckOutDate = cout;
            discount = 0;
            bedsSmall = 0;
            bedsBig = 0;
            //Last one must be setter, since setters invoke this.CalculatePrice()
            Quantity = q;
        }
        
        public void CalculatePrice()
        {
            int checkInSeasson,checkOutSeasson;
            //if Price[i] gets addressed PropertyChanged is not working. Why? Setter is not invoked that way?
            ///priceArray[0] - room price out of the seasson
            ///priceArray[1] - room price in low seasson
            ///priceArray[2] - room price in high seasson
            ///priceArray[3] - total item price
            decimal[] priceArray = new decimal[4];
            DateTime date0 = new DateTime(CheckInDate.Year, mainWindow.seassons[0].month, mainWindow.seassons[0].day);
            DateTime date1 = new DateTime(CheckInDate.Year, mainWindow.seassons[1].month, mainWindow.seassons[1].day);
            DateTime date2 = new DateTime(CheckInDate.Year, mainWindow.seassons[2].month, mainWindow.seassons[2].day);
            DateTime date3 = new DateTime(CheckInDate.Year, mainWindow.seassons[3].month, mainWindow.seassons[3].day);

            ///Determine in which tourist seassons are Check-In and Check-Out dates
            ///No need to validate the dates since UI does that
            if (CheckInDate.CompareTo(date0) < 0) checkInSeasson = 0;
            else if (CheckInDate.CompareTo(date1) < 0) checkInSeasson = 1;
            else if (CheckInDate.CompareTo(date2) < 0) checkInSeasson = 2;
            else if (CheckInDate.CompareTo(date3) < 0) checkInSeasson = 3;
            else checkInSeasson = 0;
            if (CheckOutDate.CompareTo(date0) < 0) checkOutSeasson = 0;
            else if (CheckOutDate.CompareTo(date1) < 0) checkOutSeasson = 1;
            else if (CheckOutDate.CompareTo(date2) < 0) checkOutSeasson = 2;
            else if (CheckOutDate.CompareTo(date3) < 0) checkOutSeasson = 3;
            else checkOutSeasson = 0;

            if(checkInSeasson == checkOutSeasson)
            {
                Days[checkInSeasson] = (int)(Math.Round((CheckOutDate - CheckInDate).TotalHours) / 24);
                switch (checkInSeasson)
                {
                    case 0:
                        priceArray[checkInSeasson] = Days[checkInSeasson] * MainWindow.rooms[this.Name].Out;
                        break;
                    case 1:
                        priceArray[checkInSeasson] = Days[checkInSeasson] * MainWindow.rooms[this.Name].Low;
                        break;
                    case 2:
                        priceArray[checkInSeasson] = Days[checkInSeasson] * MainWindow.rooms[this.Name].High;
                        break;
                    case 3:
                        priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
                        break;

                }
            }
            else if(checkInSeasson == 0 && checkOutSeasson == 1)
            {
                Days[0] = (int)(Math.Round((date0 - CheckInDate).TotalHours) / 24);
                priceArray[0] = Days[0] * MainWindow.rooms[this.Name].Out;
                Days[1] = (int)(Math.Round((CheckOutDate - date0).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
            }
            else if (checkInSeasson == 1 && checkOutSeasson == 2)
            {
                Days[1] = (int)(Math.Round((date1 - CheckInDate).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
                Days[2] = (int)(Math.Round((CheckOutDate - date1).TotalHours) / 24);
                priceArray[2] = Days[2] * MainWindow.rooms[this.Name].High;
            }
            else if (checkInSeasson == 2 && checkOutSeasson == 3)
            {
                Days[2] = (int)(Math.Round((date2 - CheckInDate).TotalHours) / 24);
                priceArray[2] = Days[2] * MainWindow.rooms[this.Name].High;
                Days[1] = (int)(Math.Round((CheckOutDate - date2).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
            }
            else if (checkInSeasson == 3 && checkOutSeasson == 0)
            {
                Days[1] = (int)(Math.Round((date3 - CheckInDate).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
                Days[0] = (int)(Math.Round((CheckOutDate - date3).TotalHours) / 24);
                priceArray[0] = Days[0] * MainWindow.rooms[this.Name].Out;
            }
            else if (checkInSeasson == 0 && checkOutSeasson == 2)
            {
                Days[0] = (int)(Math.Round((CheckInDate - date0).TotalHours) / 24);
                priceArray[0] = Days[0] * MainWindow.rooms[this.Name].Out;
                Days[1] = (int)(Math.Round((date1 - date0).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
                Days[2] = (int)(Math.Round((CheckOutDate - date1).TotalHours) / 24);
                priceArray[2] = Days[2] * MainWindow.rooms[this.Name].High;
            }
            else if (checkInSeasson == 1 && checkOutSeasson == 3)
            {
                Days[1] = (int)(Math.Round((date1 - CheckInDate).TotalHours) / 24);
                Days[2] = (int)(Math.Round((date2 - date1).TotalHours) / 24);
                priceArray[2] = Days[2] * MainWindow.rooms[this.Name].High;
                Days[1] += (int)(Math.Round((CheckOutDate - date2).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
            }
            else if (checkInSeasson == 2 && checkOutSeasson == 0)
            {
                Days[2] = (int)(Math.Round((date2 - CheckInDate).TotalHours) / 24);
                priceArray[2] = Days[2] * MainWindow.rooms[this.Name].High;
                Days[1] = (int)(Math.Round((date3 - date2).TotalHours) / 24);
                priceArray[1] = Days[1] * MainWindow.rooms[this.Name].Low;
                Days[0] = (int)(Math.Round((CheckOutDate - date3).TotalHours) / 24);
                priceArray[0] = Days[0] * MainWindow.rooms[this.Name].Out;
            }

            //Adding the cost for the additional beds and multiplying it by the number of nights
            for (int i = 0; i < 3; i++)
            {
                if (priceArray[i] > (decimal)0)
                {
                    decimal priceForOneGuest = priceArray[i] / MainWindow.rooms[this.Name].Guests;
                    priceArray[i] *= this.quantity;
                    if (BedsBig > 0) priceArray[i] += priceForOneGuest * (mainWindow.Settings[(int)MainWindow.Setting.addBedBigPrice] / (decimal)100) * (decimal)BedsBig;
                    if (BedsSmall > 0) priceArray[i] += priceForOneGuest * (mainWindow.Settings[(int)MainWindow.Setting.addBedSmallPrice] / (decimal)100) * (decimal)BedsSmall;
                    priceArray[i] = Decimal.Round(priceArray[i], 2);
                }
            }
            priceArray[3] = priceArray[0] + priceArray[1] + priceArray[2];
            Days[3] = Days[0] + Days[1] + Days[2];
            priceArray[3] -= ((Discount/(decimal)100) * priceArray[3]);
            priceArray[3] = Decimal.Round(priceArray[3], 2);
            Price = priceArray;
        }
    }
}
