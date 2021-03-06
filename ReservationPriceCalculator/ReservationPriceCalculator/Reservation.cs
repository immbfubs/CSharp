﻿using System;
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

        //If an item with the same name, checkIn and checkOut dates exists,
        //it only adds the new item's quantity to the existing item's quantity
        public void AddToReservation(Item itm)
        {
            Add(itm);
            TotalRefresh();
        }

		//This method also looks after the empty pirce columns and hides them.
		//When item is added to reservarion this method is called twice.
		//Once from this.AddToReservation() and once from the Item.Quantity setter.
		//Is it possible to avoid that?
		public void TotalRefresh()
		{
			MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

			this.Total = 0;
			bool[] priceIsZero = { true, true, true };
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
